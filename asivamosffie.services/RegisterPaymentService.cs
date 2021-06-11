using asivamosffie.model.AditionalModels;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            _userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        /// <summary>
        /// Get main tables
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePagosRendimientos> lista = await _context.CarguePagosRendimientos
                .Where(order => !order.Eliminado).AsNoTracking().ToListAsync();

            lista.Where(w => w.TipoCargue == typeFile && (string.IsNullOrEmpty(status) || w.EstadoCargue == status))
                .ToList().ForEach(c =>
                {
                    listaContrats.Add(new
                    {
                        c.CargaPagosRendimientosId,
                        c.NombreArchivo,
                        c.Json,
                        c.Observaciones,
                        c.TotalRegistros,
                        c.RegistrosValidos,
                        c.RegistrosInvalidos,
                        c.CargueValido,
                        c.FechaCargue,
                        c.RegistrosInconsistentes,
                        c.RegistrosConsistentes,
                        c.MostrarInconsistencias,
                        c.FechaTramite,
                        c.PendienteAprobacion
                    });
                });

            return listaContrats;
        }


        /// <summary>
        /// Validate and Upload Payments and Performances
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="fileType"></param>
        /// <param name="saveSuccessProcess"></param>
        /// 
        /// <returns></returns>        
        public async Task<Respuesta> UploadFileToValidate(IFormFile pFile, string fileType, bool saveSuccessProcess)
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
                List<Dictionary<string, string>> listaCarguePagosRendimientos = new List<Dictionary<string, string>>();
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
                    Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();

                    bool tieneError = false;
                    try
                    {
                        
                        var validatedValues = await ValidatePaymentFile(worksheet, indexWorkSheetRow, paymentNumbers);
                        carguePagosRendimiento = validatedValues.list;
                        if (validatedValues.errors != null)
                            errors.AddRange(validatedValues.errors);
                        tieneError = validatedValues.errors.Count > 0;
                       

                        if (tieneError)
                        {
                            CantidadRegistrosInvalidos++;
                        }

                        // accounts.Add(carguePagosRendimiento.GetValueOrDefault("Número de Cuenta"));
                        listaCarguePagosRendimientos.Add(carguePagosRendimiento);
                    }
                    catch (Exception ex)
                    {
                        CantidadRegistrosInvalidos++;
                    }
                }
                CarguePagosRendimientos carguePagosRendimientos = new CarguePagosRendimientos();
                int cantidadRegistrosTotales = worksheet.Dimension.Rows - 1;

                if (CantidadRegistrosInvalidos > 0 || saveSuccessProcess)
                {
                    carguePagosRendimientos = new CarguePagosRendimientos
                    {
                       // EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
                        CargueValido = CantidadRegistrosInvalidos == 0,
                        NombreArchivo = pFile.FileName,
                        RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                        RegistrosInvalidos = CantidadRegistrosInvalidos,
                        TotalRegistros = cantidadRegistrosTotales,
                        TipoCargue = fileType,
                        FechaCargue = DateTime.Now,
                        Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos),
                        UsuarioCreacion = _userName,
                        Errores = JsonConvert.SerializeObject(errors),
                    };

                    _context.CarguePagosRendimientos.Add(carguePagosRendimientos);
                }
                bool isValidPayment = true;
                if (CantidadRegistrosInvalidos == 0 && saveSuccessProcess)
                {
                    try
                    {
                        isValidPayment = await ProcessPayment(fileType, listaCarguePagosRendimientos, carguePagosRendimientos);
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
        /// <param name="carguePagosRendimiento"></param>
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
                    errors.Add(new ExcelError(indexWorkSheetRow, 2, $"El orden de giro  número{cellValue}, no tiene una Solicitud de Pago válida"));
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
            var cellType = worksheet.Cells[indexWorkSheetRow, 2].Value.GetType().Name;
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

            if (dateCellValue != dateObje.ToString() || cellType == "DateTime" || cellType == "Double")
            {
                carguePagosRendimiento.Add("Fecha de pago", guideDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
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
                var solicitudQuery = await (from solicitud in _context.SolicitudPago
                                            from sRegistroSolPago in solicitud.SolicitudPagoRegistrarSolicitudPago
                                            from sFase in sRegistroSolPago.SolicitudPagoFase
                                            from factura in sFase.SolicitudPagoFaseFactura
                                            from descuento in factura.SolicitudPagoFaseFacturaDescuento
                                            where solicitud.OrdenGiroId == ordenGiro.OrdenGiroId &&
                                            sFase.EsPreconstruccion == false
                                            select descuento).AsNoTracking().ToListAsync();

                var valorTotalDescuentos = solicitudQuery.Sum(x => x.ValorDescuento);

                if (solicitudQuery == null || taxDecimal != valorTotalDescuentos)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 3, "Por favor ingresa el valor de Impuesto que coincida con la Solicitud de Pago almacenada"));
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
        /// <param name="typeFile"></param>
        /// <param name="listaCarguePagosRendimientos"></param>
        public async Task<bool> ProcessPayment(string typeFile, List<Dictionary<string, string>> listaCarguePagosRendimientos,
            CarguePagosRendimientos carguePagosRendimientos)
        {
            foreach (var payment in listaCarguePagosRendimientos)
            {
               
                    string cellValue = payment["Número de orden de giro"];
                    var solicitud = await _context.SolicitudPago.Where(
                    x => x.NumeroSolicitud == cellValue)
                    .Include(solicitud => solicitud.OrdenGiro)
                    .ThenInclude(detalle => detalle.OrdenGiroDetalle)
                    .ThenInclude(causacion => causacion.OrdenGiroDetalleTerceroCausacion)
                    .ThenInclude(detalle => detalle.OrdenGiroDetalleTerceroCausacionAportante)
                    .AsNoTracking().FirstOrDefaultAsync();

                    var OrdenGiroDetalleTerceroCausacionAportante = solicitud?.OrdenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.
                        OrdenGiroDetalleTerceroCausacion.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacionAportante.FirstOrDefault();
                    var gestionFuenteFinanciacionId = (int)OrdenGiroDetalleTerceroCausacionAportante.FuenteFinanciacionId;

                    var valorSolicitado = payment["Valor neto girado"];
                    var pDisponibilidadPresObservacion = new GestionFuenteFinanciacion();
                    pDisponibilidadPresObservacion.FuenteFinanciacionId = gestionFuenteFinanciacionId;
                    pDisponibilidadPresObservacion.ValorSolicitado = decimal.Parse(valorSolicitado);
                    pDisponibilidadPresObservacion.UsuarioCreacion = _userName;
                    var valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == pDisponibilidadPresObservacion.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                    var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                    pDisponibilidadPresObservacion.SaldoActual = (decimal)fuente.ValorFuente - valoresSolicitados;
                    pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;
                    int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                    pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresObservacion.EstadoCodigo = estado.ToString();
                    pDisponibilidadPresObservacion.Eliminado = false;
                    _context.GestionFuenteFinanciacion.Add(pDisponibilidadPresObservacion);


                    // cruce de pagos
                    var paymentOrder = new OrdenGiroPago();
                    paymentOrder.OrdenGiroId = solicitud.OrdenGiro.OrdenGiroId;
                    carguePagosRendimientos.OrdenGiroPago.Add(paymentOrder);
                    // paymentOrder.RegistroPagoId = payment.;

                    //    carguePagosRendimientos.
                    //    _context.OrdenGiroPago.Add(paymentOrder);
            }
            return true;
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
