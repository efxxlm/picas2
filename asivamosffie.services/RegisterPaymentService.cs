using asivamosffie.model.AditionalModels;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Extensions;
using asivamosffie.services.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterPaymentService : IRegisterPaymentService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        public readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userName;
        public ISourceFundingService _fundingSourceService { get; }
        public MailSettings _mailSettings { get; }
        public readonly int _500 = 500;

        public RegisterPaymentService(IConverter converter,
                                            devAsiVamosFFIEContext context,
                                            ISourceFundingService fundingSourceService,
                                            ICommonService commonService,
                                            IDocumentService documentService,
                                            IOptions<MailSettings> mailSettings,
                                            IHttpContextAccessor httpContextAccessor
            )
        {
            _converter = converter;
            _context = context;
            _fundingSourceService = fundingSourceService;
            _documentService = documentService;
            _mailSettings = mailSettings.Value;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _userName = _httpContextAccessor.HttpContext.User.Identity.Name.ToUpper();
        }

        /// <summary>
        /// Get main tables
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetPayments(string status)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePago> lista = await _context.CarguePago
                .Where(order => !order.Eliminado).AsNoTracking().ToListAsync();

            lista.Where(w => (string.IsNullOrEmpty(status)))
                .ToList().ForEach(c =>
                {
                    listaContrats.Add(new
                    {
                        c.CarguePagoId,
                        c.NombreArchivo,
                        c.JsonContent,
                        c.Observaciones,
                        c.TotalRegistros,
                        c.RegistrosValidos,
                        c.RegistrosInvalidos,
                        c.CargueValido,
                        c.FechaCargue
                    });
                });

            return listaContrats;
        }


        /// <summary>
        /// Validate and Upload Payments and Performances
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="saveSuccessProcess"></param>
        /// 
        /// 
        /// <returns></returns>        
        public async Task<Respuesta> UploadFileToValidate(IFormFile pFile, bool saveSuccessProcess)
        {
            int CantidadRegistrosInvalidos = 0;
            int idAction = await GetActionIdAudit(ConstantCodigoAcciones.Validar_Excel_Registro_Pagos);
            string actionMesage = ConstantCommonMessages.Performances.REGISTRAR_ORDENES_PAGOS;
            if (pFile == null)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = GeneralCodes.EntradaInvalida,
                    Message = await SaveAuditAction(_userName, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage)
                };
            }

            using (var stream = new MemoryStream())
            {
                List<Dictionary<string, string>> listPayments = new List<Dictionary<string, string>>();
                List<ExcelError> errors = new List<ExcelError>();

                await pFile.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // TODO add validation to prevent query a column in a  not existing position

                var columnAccounst = worksheet.Cells[2, 2, worksheet.Dimension.Rows, 2].Select(v => v.Text).ToList();

              
                bool accountsDifferents = columnAccounst.Count != worksheet.Dimension.Rows - 1;

                // Query payment 
                var paymentNumColum = worksheet.Cells[2, 1, worksheet.Dimension.Rows, 1].Select(v => v.Text).ToList();
                HashSet<string> paymentNumbers = new HashSet<string>();

                //Controlar Registros

                //TODO ADD PARALLEL FOREACH

                for (int indexWorkSheetRow = 2; indexWorkSheetRow <= worksheet.Dimension.Rows; indexWorkSheetRow++)
                {
                    Dictionary<string, string> uploadPayment = new Dictionary<string, string>();

                    bool tieneError = false;
                    try
                    {
                        
                        var validatedValues = await ValidatePaymentFile(worksheet, indexWorkSheetRow, paymentNumbers);
                        uploadPayment = validatedValues.list;
                        if (validatedValues.errors != null)
                            errors.AddRange(validatedValues.errors);
                        tieneError = validatedValues.errors.Count > 0;
                       

                        if (tieneError)
                        {
                            CantidadRegistrosInvalidos++;
                        }

                        // accounts.Add(carguePagosRendimiento.GetValueOrDefault("Número de Cuenta"));
                        listPayments.Add(uploadPayment);
                    }
                    catch (Exception ex)
                    {
                        CantidadRegistrosInvalidos++;
                    }
                }
                CarguePago carguePago = new CarguePago();
                int cantidadRegistrosTotales = worksheet.Dimension.Rows - 1;

                if (CantidadRegistrosInvalidos > 0 || saveSuccessProcess)
                {
                    carguePago = new CarguePago
                    {
                       // EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
                        CargueValido = CantidadRegistrosInvalidos == 0,
                        NombreArchivo = pFile.FileName,
                        RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                        RegistrosInvalidos = CantidadRegistrosInvalidos,
                        TotalRegistros = cantidadRegistrosTotales,
                        FechaCargue = DateTime.Now,
                        JsonContent = JsonConvert.SerializeObject(listPayments),
                        UsuarioCreacion = _userName,
                        Errores = JsonConvert.SerializeObject(errors),
                    };

                    _context.CarguePago.Add(carguePago);
                }
                bool isValidPayment = true;
                if (CantidadRegistrosInvalidos == 0 && saveSuccessProcess)
                {
                    try
                    {
                        isValidPayment = await ProcessPayment(listPayments, carguePago);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                if (isValidPayment)
                {
                    _context.SaveChanges();
                }

                ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                {
                    CantidadDeRegistros = cantidadRegistrosTotales.ToString(),
                    CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                    CantidadDeRegistrosValidos = (cantidadRegistrosTotales - CantidadRegistrosInvalidos).ToString(),
                    LlaveConsulta = pFile.FileName
                };

                return new Respuesta
                {
                    Data = archivoCargueRespuesta,
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await SaveAuditAction(_userName, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage)
                };
            }
        }


        /// <summary>
        /// Validar que la Orden de Giro exista, corresponda con el valor pagado, 
        /// con el valor que se encuentra en el archivo.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="indexWorkSheetRow"></param>
        /// <returns></returns>
        private async Task<(Dictionary<string, string> list, List<ExcelError> errors)> ValidatePaymentFile(ExcelWorksheet worksheet,
            int indexWorkSheetRow, HashSet<string> paymentNumbers)
        {
            Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
            List<ExcelError> errors = new List<ExcelError>();
            // Validator.Validate(worksheet, indexWorkSheetRow, PaymentValidations.validations);
            // #1
            //Número de orden de giro
            string cellValue = worksheet.Cells[indexWorkSheetRow, 1].Text;
            carguePagosRendimiento.Add("Número de orden de giro", cellValue);
            OrdenGiro ordenGiro = new OrdenGiro();


            if (string.IsNullOrEmpty(cellValue) || cellValue.Length > 50)
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 2, "Longitud inválida"));
            }
            else
            {
                ordenGiro = await _context.OrdenGiro.Where(x => x.NumeroSolicitud == cellValue
                       && x.EstadoCodigo == ((int)EnumEstadoOrdenGiro.Con_Orden_de_Giro_Tramitada).ToString())
                    ?.Include(orden => orden.OrdenGiroDetalle)
                    .ThenInclude(detalle => detalle.OrdenGiroDetalleTerceroCausacion).AsNoTracking().FirstOrDefaultAsync();

                var valorNeto = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault()?.ValorNetoGiro;

                if (ordenGiro == null || valorNeto == null)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 2, $"El orden de giro  número{cellValue}, no tiene una Orden de Giro válida"));
                }
                else
                {
                    var processed = _context.OrdenGiroPago.Any(x => x.OrdenGiroId == ordenGiro.OrdenGiroId);
                    if (processed)
                    {
                        errors.Add(new ExcelError(indexWorkSheetRow, 2, $"El orden de giro  número{cellValue}, ya tiene un pago registrado"));
                    }
                    if (paymentNumbers.Contains(cellValue))
                    {

                        errors.Add(new ExcelError(indexWorkSheetRow, 2, $"Por favor ingrese una única vez la orden de giro, {cellValue} se encuentra más de una vez en el documento"));
                    }
                    paymentNumbers.Add(cellValue);
                }

            }

            // #2
            //Fecha de pago
            //string sDate = (worksheet.Cells[indexWorkSheetRow, 2] as DocumentFormat.OpenXml.Office.Excel.Range).Value2.ToString();
            //DateTime conv = DateTime.FromOADate(d);

            var dateObje = worksheet.Cells[indexWorkSheetRow, 2].Value;
            DateTime guideDate = new DateTime();
            var cellType = worksheet.Cells[indexWorkSheetRow, 2].Value?.GetType().Name;
            if (cellType == "DateTime")
            {
                var dtFormat = "dd/MM/yyyy";
                worksheet.Cells[indexWorkSheetRow, 1].Style.Numberformat.Format = dtFormat;
            }
            string dateText = worksheet.Cells[indexWorkSheetRow, 2].Text;
            bool isDate = Helpers.Helpers.TryStringToDate(dateText, out guideDate);

            if (cellType == "Double")
            {
                Double d = Double.Parse(dateObje.ToString());
                guideDate = DateTime.FromOADate(d);
            }

            string dateCellValue = worksheet.Cells[indexWorkSheetRow, 2].Text;

            if (dateObje != null && (dateCellValue != dateObje.ToString() || cellType == "DateTime" || cellType == "Double"))
            {
                carguePagosRendimiento.Add("Fecha de pago", guideDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                // guideDate = dateObje;
            }
            else
            {
                carguePagosRendimiento.Add("Fecha de pago", dateCellValue);
            }

            if (string.IsNullOrEmpty(dateText) || (!isDate && guideDate == DateTime.MinValue))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 3, "Por favor ingresa solo fechas en este campo"));
            }


            // #3
            //Impuestos
            carguePagosRendimiento.Add("Impuestos", worksheet.Cells[indexWorkSheetRow, 3].Text);
            string impuetoClean = worksheet.Cells[indexWorkSheetRow, 3].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[indexWorkSheetRow, 3].Text) || impuetoClean.Length > 20 || !decimal.TryParse(impuetoClean, out decimal taxDecimal))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 4, "Por favor ingresa solo datos numéricos en este campo"));
            }
            else if (ordenGiro != null)
            {
                var queryOrden = (from orden in _context.OrdenGiro.Where(x => x.OrdenGiroId == ordenGiro.OrdenGiroId)
                                  join detalle in _context.OrdenGiroDetalle.DefaultIfEmpty() on orden.OrdenGiroId equals detalle.OrdenGiroId
                                  join causacion in _context.OrdenGiroDetalleTerceroCausacion.DefaultIfEmpty() on detalle.OrdenGiroDetalleId equals causacion.OrdenGiroDetalleId
                                  join descuento in _context.OrdenGiroDetalleTerceroCausacionDescuento.DefaultIfEmpty() on causacion.OrdenGiroDetalleTerceroCausacionId equals descuento.OrdenGiroDetalleTerceroCausacionId
                                  where descuento.Eliminado == false
                                  group new { descuento } by new
                                  {
                                      orden.OrdenGiroId,
                                      orden.NumeroSolicitud,
                                      causacion.ValorNetoGiro
                                  } into g
                                  select new
                                  {
                                      g.Key.OrdenGiroId,
                                      g.Key.NumeroSolicitud,
                                      DescuentoCausacion = g.Sum(x => x.descuento.ValorDescuento),
                                  }).FirstOrDefault();

                var queryOrdenDescuentoTecnica = (from orden in _context.OrdenGiro.Where(x => x.OrdenGiroId == ordenGiro.OrdenGiroId)
                                                  join detalle in _context.OrdenGiroDetalle.DefaultIfEmpty() on orden.OrdenGiroId equals detalle.OrdenGiroId
                                                  join tecnica in _context.OrdenGiroDetalleDescuentoTecnica.DefaultIfEmpty() on detalle.OrdenGiroDetalleId equals tecnica.OrdenGiroDetalleId
                                                  join aportante in _context.OrdenGiroDetalleDescuentoTecnicaAportante.DefaultIfEmpty() on tecnica.OrdenGiroDetalleDescuentoTecnicaId equals aportante.OrdenGiroDetalleDescuentoTecnicaId
                                                  where aportante.Eliminado == false
                                                  group new { aportante } by new
                                                  {
                                                      orden.OrdenGiroId,
                                                      orden.NumeroSolicitud,
                                                  } into g
                                                  select new
                                                  {
                                                      g.Key.OrdenGiroId,
                                                      g.Key.NumeroSolicitud,
                                                      DescuentoTecnica = g.Sum(x => x.aportante.ValorDescuento)
                                                  }).FirstOrDefault();

                var valorTotalDescuentos = queryOrdenDescuentoTecnica.DescuentoTecnica + queryOrden.DescuentoCausacion;

                if (queryOrden == null || taxDecimal != valorTotalDescuentos)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 4, "Por favor ingresa el valor de Impuesto que coincida con la Orden de Giro almacenada"));
                }

            }


            // #4
            //Valor neto girado
            carguePagosRendimiento.Add("Valor neto girado", worksheet.Cells[indexWorkSheetRow, 4].Text);
            string valorNetoClean = worksheet.Cells[indexWorkSheetRow, 4].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[indexWorkSheetRow, 4].Text) || valorNetoClean.Length > 20 || !decimal.TryParse(valorNetoClean, out decimal respValNetoNumber))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 5, "Por favor ingresa solo datos numéricos en este campo"));
            }
            else if (ordenGiro != null)
            {
                var valorNeto = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault()?.ValorNetoGiro;
                if (!valorNeto.HasValue || valorNeto.Value != respValNetoNumber)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 5, "La orden de giro tiene un valor diferente"));
                }
            }

            return (carguePagosRendimiento, errors);
        }

        /// <summary>
        /// Al guardar la información para los registros válidos, 
        /// el sistema deberá afectar los saldos de las cuentas asociadas 
        /// a las órdenes de giro especificadas en el archivo, de acuerdo 
        /// con los valores referenciados en cada una de ellas.
        /// La fuente de financiación tiene una cuenta bancaria y cada fuente está asociada a un aportan
        /// </summary>
        /// <param name="listPayments"></param>
        public async Task<bool> ProcessPayment(List<Dictionary<string, string>> listPayments,
            CarguePago carguePagos)
        {
            foreach (var payment in listPayments)
            {               
                string cellValue = payment["Número de orden de giro"];
                var ordenGiro = await _context.OrdenGiro.Where(
                x => x.NumeroSolicitud == cellValue)
                .Include(detalle => detalle.OrdenGiroDetalle)
                .ThenInclude(causacion => causacion.OrdenGiroDetalleTerceroCausacion)
                .ThenInclude(detalle => detalle.OrdenGiroDetalleTerceroCausacionAportante)
                .ThenInclude(aportante => aportante.Aportante)
                .AsNoTracking().FirstOrDefaultAsync();

                var OrdenGiroDetalleTerceroCausacionAportante = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.
                    OrdenGiroDetalleTerceroCausacion.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacionAportante.FirstOrDefault();
                var gestionFuenteFinanciacionId = (int)OrdenGiroDetalleTerceroCausacionAportante.FuenteFinanciacionId;
                var aportante = OrdenGiroDetalleTerceroCausacionAportante?.Aportante;

                
                var valorSolicitado = payment["Valor neto girado"];
                var gestionFuenteFinanciacion = new GestionFuenteFinanciacion();
                gestionFuenteFinanciacion.FuenteFinanciacionId = gestionFuenteFinanciacionId;
                gestionFuenteFinanciacion.ValorSolicitado = decimal.Parse(valorSolicitado);
                gestionFuenteFinanciacion.UsuarioCreacion = _userName;
                var fuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestionFuenteFinanciacionId)
                    .Include(f => f.ControlRecurso).AsNoTracking().FirstOrDefault();
                var valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == gestionFuenteFinanciacionId).Sum(x => x.ValorSolicitado);

                decimal? rendimientos = 0;
                // 1. Al registrar el pago, se afecta el saldo de la fuente, para el caso de aportantes diferentes al FFIE, 
                // para saber el saldo que hay tener en cuenta el valor que hay en el campo
                // "Valor de aporte en cuenta" + "REndimientos incorporados".
                // 2 Al registrar el pago, se afecta el saldo de la fuente, para el caso de ser aportante FFIE, 
                // para saber el saldo que hay tener en cuenta el valor que hay en el campo "Aporte en fuente" + 
                // "REndimientos incorporados".
                if (aportante != null && aportante.TipoAportanteId == ConstanTipoAportante.Ffie)
                {
                    var saldosFuente = await _context.VSaldosFuenteXaportanteId.Where(r => r.CofinanciacionAportanteId == aportante.CofinanciacionAportanteId).ToListAsync();
                    rendimientos = saldosFuente.Sum(x => x.RendimientosIncorporados);
                    gestionFuenteFinanciacion.SaldoActual = ((decimal)fuente.ValorFuente + (rendimientos ?? rendimientos.Value)) - valoresSolicitados;

                }
                else
                {
                    decimal valorAporteEnCuenta = 0;
                    foreach (var control in fuente.ControlRecurso)
                    {
                        control.FuenteFinanciacion = null; 
                    }

                    foreach (var element in fuente.ControlRecurso.Where(
                        x => x.FuenteFinanciacionId == fuente.FuenteFinanciacionId))
                    {
                        valorAporteEnCuenta += element.ValorConsignacion;
                    }

                    gestionFuenteFinanciacion.SaldoActual = ((decimal)valorAporteEnCuenta + ( rendimientos ?? rendimientos.Value)) - valoresSolicitados;

                }

                gestionFuenteFinanciacion.NuevoSaldo = gestionFuenteFinanciacion.SaldoActual - gestionFuenteFinanciacion.ValorSolicitado;
                int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                gestionFuenteFinanciacion.FechaCreacion = DateTime.Now;
                gestionFuenteFinanciacion.EstadoCodigo = estado.ToString();
                gestionFuenteFinanciacion.Eliminado = false;
                _context.GestionFuenteFinanciacion.Add(gestionFuenteFinanciacion);


                // cruce de pagos
                var paymentOrder = new OrdenGiroPago();
                paymentOrder.OrdenGiroId = ordenGiro.OrdenGiroId;
                carguePagos.OrdenGiroPago.Add(paymentOrder);
            }
            return true;
        }




        public async Task<Respuesta> SetObservationPayments(string observaciones, int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.OBSERVACIONES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Comentar_Pagos);
            int modifiedRows = 0;
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                response.Code = GeneralCodes.CamposVacios;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage);
                return response;
            }

            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePago>()
                      .Where(order => order.CarguePagoId == uploadedOrderId)
                      .UpdateAsync(o => new CarguePago()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = _userName,
                          Observaciones = observaciones
                      });

            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : GeneralCodes.EntradaInvalida;

            response.IsSuccessful = true;
            response.Code = codeResponse;
            response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage);

            return response;
        }


        /// <summary>
        /// TODO validar que este no tenga relaciones o información dependiente. 
        /// En caso de contar con alguna relación, el sistema debe informar al usuario
        /// por medio de un mensaje emergente “El registro tiene información que depende de él, no se puede eliminar”
        /// , y no deberá eliminar el registro.
        /// </summary>
        /// <param name="cargaPagosRendimientosId"></param>
        /// <param name="uploadStatus"></param>
        /// <returns></returns>
        public async Task<Respuesta> DeletePayment(int uploadedOrderId)
        {
            string actionMesage = ConstantCommonMessages.Performances.ELIMINAR_ORDENES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Eliminar_Pagos);
            int modifiedRows = -1;
            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePago>()
                      .Where(order => order.CarguePagoId == uploadedOrderId)
                      .UpdateAsync(o => new CarguePago()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = _userName,
                          Eliminado = true
                      });
            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : ConstMessagesPerformances.ErrorGuardarCambios;

            return new Respuesta
            {
                Data = modifiedRows,
                IsSuccessful = modifiedRows > 0,
                IsException = false,
                IsValidation = false,
                Code = GeneralCodes.OperacionExitosa,
                Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage)
            };
        }

        #region FileValidation
        private string CheckFileDownloadDirectory()
        {
            string directory = Path.Combine(_mailSettings.DirectoryBase,
                                                   _mailSettings.DirectoryBaseCargue,
                                                   _mailSettings.DirectoryBasePagos);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
        #endregion

        public async Task<Respuesta> DownloadPaymentsAsync(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string action = ConstantCodigoAcciones.Ver_Detalle_Pagos;
            int actionId = await GetActionIdAudit(action);
            string actionMesage = ConstantCommonMessages.Performances.VER_DETALLE_PAGOS;
            string directory = CheckFileDownloadDirectory();
            string filePath = string.Empty;
            string jsonString = string.Empty;
            try
            {
                var collection = _context.CarguePago.Where(x => x.CarguePagoId == uploadedOrderId)
                            .Select(
                            order => new
                            {
                                Estado = order.CargueValido,
                                ArchivoJson = order.JsonContent,
                                Errores = order.Errores
                            }).FirstOrDefault();
                jsonString = collection.ArchivoJson;

                List<ExcelError> errors = new List<ExcelError>();
                if (collection.Errores != null)
                    errors = JsonConvert.DeserializeObject<List<ExcelError>>(collection.Errores);

               
                List<EntryPaymentOrder> list = SerializePaymentOrders(uploadedOrderId, collection.ArchivoJson);
                filePath = FileToPath.WriteCollectionToPath("Pagos", directory, list, errors);
                
               
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = ConstMessagesPayments.ErrorDescargarArchivo;
                response.Data = jsonString;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        response.Code,
                                         ex.SubstringValid(_500));
                return response;

            }
            catch (Exception ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = ConstMessagesPayments.ErrorDescargarArchivo;
                response.Data = jsonString;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        response.Code,
                                         ex.SubstringValid(_500));

                return response;

            }

            response.Data = filePath;
            response.IsSuccessful = true;
            response.Code = GeneralCodes.OperacionExitosa;
            response.Message = await SaveAuditAction(_userName, actionId,
                                    enumeratorMenu.RegistrarPagosRendimientos,
                                    GeneralCodes.OperacionExitosa,
                                    actionMesage);

            return response;
        }

        private List<EntryPaymentOrder> SerializePaymentOrders(int uploadedOrderId, string stringJson)
        {
            var list = JsonConvert.DeserializeObject<List<EntryPaymentOrder>>(stringJson);
            return list;
        }

        private async Task<int> GetActionIdAudit(string action)
        {
            return await _commonService.GetDominioIdByCodigoAndTipoDominio(
                            action, (int)EnumeratorTipoDominio.Acciones);
        }

        private async Task<string> SaveAuditAction(string author, int idAction, enumeratorMenu menu, string code, string comment)
        {
            return await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                     (int)menu, code, idAction, author, comment);
        }

    }
}
