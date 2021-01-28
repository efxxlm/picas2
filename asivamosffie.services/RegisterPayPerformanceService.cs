using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf.Contracts;
using System.Globalization;
using OfficeOpenXml;
using Z.EntityFramework.Plus;
using Microsoft.Extensions.Options;
using asivamosffie.model.AditionalModels;
using asivamosffie.services.Helpers.Extensions;
using Microsoft.VisualBasic;
using DinkToPdf;

namespace asivamosffie.services
{
    public partial class RegisterPayPerformanceService
    {
        #region Audit
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
        #endregion

        #region Validation
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

        private static string WriteCollectionToPath<T>(string fileName, string directory, List<T> list)
        {
            string filePath;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var streams = new MemoryStream();
            using (var packages = new ExcelPackage())
            {
                var workSheet = packages.Workbook.Worksheets.Add("Hoja 1");
                workSheet.Cells["A1"].LoadFromCollection(list, true);
                packages.Save();
                ////convert the excel package to a byte array
                byte[] bin = packages.GetAsByteArray();
                Stream stream = new MemoryStream(bin);
                ////the path of the file
                filePath = directory + "/" + fileName + "_rev.xlsx";

                ////write the file to the disk
                File.WriteAllBytes(filePath, bin);
            }

            return filePath;
        }

        public bool SendMail(string template, string subject, EnumeratorPerfil enumeratorProfile)
        {
            bool isMailSent = false;
            var usertoSend = _context.UsuarioPerfil.Where(
                       x => x.PerfilId == (int)EnumeratorPerfil.CordinadorFinanciera).Include(y => y.Usuario);
            foreach (var fiduciariaEmail in usertoSend)
            {
                isMailSent = Helpers.Helpers.EnviarCorreo(fiduciariaEmail.Usuario.Email,
                    subject,
                    template,
                    _mailSettings.Sender,
                    _mailSettings.Password,
                    _mailSettings.MailServer,
                    _mailSettings.MailPort);
            }
            return isMailSent;
        }

    }

    public partial class RegisterPayPerformanceService : IRegisterPayPerformanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        public readonly IBudgetAvailabilityService _budgetAvailabilityService;
        public ISourceFundingService _fundingSourceService { get; }
        public AppSettings _mailSettings { get; }
        public readonly string CONSTPAGOS = "Pagos";
        public readonly int _500 = 500;

        public RegisterPayPerformanceService(IConverter converter,
                                            devAsiVamosFFIEContext context,
                                            ISourceFundingService fundingSourceService,
                                            ICommonService commonService,
                                            IDocumentService documentService,
                                            IOptions<AppSettings> mailSettings
            )
{
            _converter = converter;
            _context = context;
            _fundingSourceService = fundingSourceService;
            _documentService = documentService;
            _mailSettings = mailSettings.Value;
            _commonService = commonService;
        }

        #region loads

        /// <summary>
        /// Get main tables
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePagosRendimientos> lista = await _context.CarguePagosRendimientos
                .Where(order => !order.Eliminado).ToListAsync();

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
                        c.FechaTramite
                    });
                });

            return listaContrats;
        }

        /// <summary>
        /// Validate and Upload Payments and Performances
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="author"></param>
        /// <param name="fileType"></param>
        /// <param name="saveSuccessProcess"></param>
        /// <returns></returns>        
        public async Task<Respuesta> UploadFileToValidate(IFormFile pFile, string author, string fileType, bool saveSuccessProcess)
        {
            int CantidadRegistrosInvalidos = 0;
            int idAction = await GetActionIdAudit(ConstantCodigoAcciones.Validar_Excel_Registro_Pagos);
            string actionMesage = fileType == CONSTPAGOS ? 
                ConstantCommonMessages.Performances.REGISTRAR_ORDENES_PAGOS : ConstantCommonMessages.Performances.REGISTRAR_RENDIMIENTOS;
            if (pFile == null)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = GeneralCodes.EntradaInvalida,
                    Message = await SaveAuditAction(author, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage)
                };
            }

            using (var stream = new MemoryStream())
            {
                List<Dictionary<string, string>> listaCarguePagosRendimientos = new List<Dictionary<string, string>>();

                await pFile.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // TODO add validation to prevent query a column in a  not existing position
                var columnAccounst = worksheet.Cells[2, 2, worksheet.Dimension.Rows, 2].Select(v => v.Text).ToList<string>();

                var bankAccounts = _context.CuentaBancaria.Where(
                    x => x.NumeroCuentaBanco != null && columnAccounst.Contains(x.NumeroCuentaBanco)).AsNoTracking();

                //Controlar Registros

                //TODO ADD PARALLEL FOREACH

                for (int indexWorkSheetRow = 2; indexWorkSheetRow <= worksheet.Dimension.Rows; indexWorkSheetRow++)
                {
                    Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
                    bool tieneError = false;
                    try
                    {
                        if (fileType == CONSTPAGOS)
                        {
                            tieneError = ValidatePaymentFile(worksheet, indexWorkSheetRow, carguePagosRendimiento);
                        }
                        else if (fileType == "Rendimientos")
                        {
                            var validatedValues = ValidateFinancialPerformanceFile(worksheet, indexWorkSheetRow, bankAccounts);

                            carguePagosRendimiento = validatedValues.list;
                            tieneError = validatedValues.hasError;
                        }

                        if (tieneError)
                        {
                            CantidadRegistrosInvalidos++;
                            carguePagosRendimiento.Add("Estado", "Fallido");
                        }
                        else
                        {
                            carguePagosRendimiento.Add("Estado", "Valido");
                        }

                        // accounts.Add(carguePagosRendimiento.GetValueOrDefault("Número de Cuenta"));
                        listaCarguePagosRendimientos.Add(carguePagosRendimiento);
                    }
                    catch (Exception ex)
                    {
                        CantidadRegistrosInvalidos++;
                    }
                }

                int cantidadRegistrosTotales = worksheet.Dimension.Rows - 1;

                if (CantidadRegistrosInvalidos > 0 || saveSuccessProcess)
                {
                    CarguePagosRendimientos CarguePagosRendimientos = new CarguePagosRendimientos
                    {
                        EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
                        CargueValido = CantidadRegistrosInvalidos == 0,
                        NombreArchivo = pFile.FileName,
                        RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                        RegistrosInvalidos = CantidadRegistrosInvalidos,
                        TotalRegistros = cantidadRegistrosTotales,
                        TipoCargue = fileType,
                        FechaCargue = DateTime.Now,
                        Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos),
                        UsuarioCreacion = author
                    };

                    _context.CarguePagosRendimientos.Add(CarguePagosRendimientos);
                    _context.SaveChanges();
                }

                if (CantidadRegistrosInvalidos == 0)
                {
                    ProcessPayment(fileType, listaCarguePagosRendimientos);
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
                    Message = await SaveAuditAction(author, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage)
                };
            }
        }

        /// <summary>
        /// TODO Y validar que la Orden de Giro exista, corresponda con el valor pagado, 
        /// con el valor que se encuentra en el archivo.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="i"></param>
        /// <param name="carguePagosRendimiento"></param>
        /// <returns></returns>
        private static bool ValidatePaymentFile(ExcelWorksheet worksheet, int i, Dictionary<string, string> carguePagosRendimiento)
        {
            bool tieneError = false;
            // #1
            //Número de orden de giro
            carguePagosRendimiento.Add("Número de orden de giro", worksheet.Cells[i, 1].Text);
            if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) || worksheet.Cells[i, 1].Text.Length > 50)
            {
                tieneError = true;
            }

            // #2
            //Fecha de pago
            carguePagosRendimiento.Add("Fecha de pago", worksheet.Cells[i, 2].Text);
            DateTime fecha;
            decimal respValNumber;
            if (string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) || !DateTime.TryParseExact(worksheet.Cells[i, 2].Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha))
            {
                tieneError = true;
            }

            // #3
            //Impuestos
            carguePagosRendimiento.Add("Impuestos", worksheet.Cells[i, 3].Text);
            string impuetoClean = worksheet.Cells[i, 3].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) || impuetoClean.Length > 20 || !decimal.TryParse(impuetoClean, out respValNumber))
            {
                tieneError = true;
            }

            // #4
            //Valor neto girado
            carguePagosRendimiento.Add("valor neto girado", worksheet.Cells[i, 4].Text);
            string valorNetoClean = worksheet.Cells[i, 4].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) || valorNetoClean.Length > 20 || !decimal.TryParse(valorNetoClean, out respValNumber))
            {
                tieneError = true;
            }

            return tieneError;
        }

        private (Dictionary<string, string> list, bool hasError) ValidateFinancialPerformanceFile(ExcelWorksheet worksheet, int indexRow, IEnumerable<CuentaBancaria> bankAccounts)
        {
            Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
            bool hasError = false;
            Dictionary<string, string> performanceStructure = new Dictionary<string, string>();
            performanceStructure.Add("Fecha de rendimientos", "Date");
            performanceStructure.Add("Número de Cuenta", "AlphaNum");
            performanceStructure.Add("Acumulado de aportes de recursos exentos", "Money");
            performanceStructure.Add("Acumulado de rendimientos exentos", "Money");
            performanceStructure.Add("Acumulado de gastos Bancarios exentos", "Money");
            performanceStructure.Add("Acumulado de gravamen financiero descontado exentos", "Money");
            performanceStructure.Add("Acumulado de aportes de recursos no exentos", "Money");
            performanceStructure.Add("Acumulado de rendimientos no exentos", "Money");
            performanceStructure.Add("Acumulado de gastos Bancarios no exentos", "Money");
            performanceStructure.Add("Acumulado de gravamen financiero descontado no exentos", "Money");

            int indexCell = 1;

           //worksheet.Cells[1, 1].AddComment("se debe eliminar una carga de flujo de inversión asociada a este Proyecto", "Admin");

            foreach (var rowFormat in performanceStructure)
            {
                string cellValue = worksheet.Cells[indexRow, indexCell].Text;
                carguePagosRendimiento.Add(rowFormat.Key, cellValue);
                if ((rowFormat.Value == "Date")
                    && (string.IsNullOrEmpty(cellValue) || !DateTime.TryParseExact(cellValue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime fecha)))
                {
                    hasError = true;
                }
                else if ((rowFormat.Value == "AlphaNum")
                    && (string.IsNullOrWhiteSpace(cellValue) || !cellValue.IsValidSize(50)))
                {
                    hasError = true;

                }
                else if ((rowFormat.Value == "Money")
                    && (string.IsNullOrWhiteSpace(cellValue) || !cellValue.IsMoneyValueValidSize(20)))
                {
                    hasError = true;

                }

                if (!hasError && rowFormat.Key == "Número de Cuenta")
                {
                    var activeAccount = bankAccounts.SingleOrDefault(account => account.NumeroCuentaBanco == cellValue);
                    hasError = activeAccount == null;
                    if (activeAccount != null)
                    {
                        hasError = !activeAccount.FuenteFinanciacionId.HasValue ||
                            (activeAccount.Eliminado.HasValue && activeAccount.Eliminado.Value);
                    }
                }

                indexCell += 1;
            }

            // carguePagosRendimiento.Add("Estado", hasError ?  "Valido" : "Fallido");

            return (carguePagosRendimiento, hasError);
        }

        #endregion

        /// <summary>
        /// TODO validar que este no tenga relaciones o información dependiente. 
        /// En caso de contar con alguna relación, el sistema debe informar al usuario
        /// por medio de un mensaje emergente “El registro tiene información que depende de él, no se puede eliminar”
        /// , y no deberá eliminar el registro.
        /// </summary>
        /// <param name="cargaPagosRendimientosId"></param>
        /// <param name="uploadStatus"></param>
        /// <returns></returns>
        public async Task<Respuesta> DeletePayment(int uploadedOrderId, string author)
        {
            string actionMesage = ConstantCommonMessages.Performances.ELIMINAR_ORDENES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Eliminar_Pagos);
            int modifiedRows = -1;
            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                      .Where(order => order.CargaPagosRendimientosId == uploadedOrderId && !order.FechaTramite.HasValue)
                      .UpdateAsync(o => new CarguePagosRendimientos()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = author,
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
                Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage)
            };
        }
        
        public async Task<Respuesta> DownloadPaymentPerformanceAsync(FileRequest fileRequest, string fileType)
        {
            Respuesta response = new Respuesta();
            string action = fileType == CONSTPAGOS ? ConstantCodigoAcciones.Ver_Detalle_Pagos : ConstantCodigoAcciones.Ver_Detalle_Rendimientos;
            int actionId = await GetActionIdAudit(action);
            string actionMesage = fileType == CONSTPAGOS ?
                ConstantCommonMessages.Performances.VER_DETALLE_PAGOS : ConstantCommonMessages.Performances.VER_DETALLE_RENDIMIENTOS;
            string directory = CheckFileDownloadDirectory();
            string filePath = string.Empty;
            string jsonString = string.Empty;
            try
            {
                var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == fileRequest.ResourceId)
                            .Select(
                            order => new
                            {
                                Estado = order.CargueValido,
                                ArchivoJson = order.Json
                            }).FirstOrDefault();
                jsonString = collection.ArchivoJson;
                if (fileType == CONSTPAGOS)
                {
                    List<PaymentOrder> list = SerializePaymentOrders(fileRequest.ResourceId, collection.ArchivoJson);
                    filePath = WriteCollectionToPath(fileRequest.FileName, directory, list);
                }
                else
                {
                    List<PerformanceOrder> list2 = SerializePerformances(fileRequest.ResourceId, collection.ArchivoJson);
                    filePath = WriteCollectionToPath(fileRequest.FileName, directory, list2);
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = GeneralCodes.OperacionExitosa;
                response.Data = jsonString;
                response.Message = await SaveAuditAction(fileRequest.Username, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage);
            }

            response.Data = filePath;
            response.IsSuccessful = true;
            response.Code = GeneralCodes.OperacionExitosa;
            response.Message = await SaveAuditAction(fileRequest.Username, actionId,
                                    enumeratorMenu.RegistrarPagosRendimientos,
                                    GeneralCodes.OperacionExitosa,
                                    actionMesage);

            return response;
        }

        private List<PerformanceOrder> SerializePerformances(int uploadedOrderId, string stringJson)
        {   
            var list = JsonConvert.DeserializeObject<List<PerformanceOrder>>(stringJson);
            return list;
        }

        private List<PaymentOrder> SerializePaymentOrders(int uploadedOrderId,string  stringJson)
        {            
            var list = JsonConvert.DeserializeObject<List<PaymentOrder>>(stringJson);
            return list;
        }

        /// <summary>
        /// Account - Número de Cuenta
        /// #2 GeneratedReturns  - Total rendimientos generados SUM x => “acumulado de rendimientos de recursos exentos”  
        ///                                              +   SUM X =>  “acumulado de  rendimientos de recursos no exentos”
        /// Current Month ?
        /// # 3 Rendimientos incorporados  //Valor de los rendimientos incorporados hasta el mes anterior (acumulado).
        /// # 4 Provisión gravamen financiero
        /// #5 Total gastos bancarios SUM  “Acumulado de gastos Bancarios exentos” y “Acumulado de gastos Bancarios no exentos”
        /// #6 Total gravamen financiero descontado “Acumulado de gravamen financiero descontado exentos” 
        ///                                       + “Acumulado de gravamen financiero descontado no exentos”
        /// #7 Visitas  // Sum payments Visitas as Concepto de Pago , see order details
        /// # 8 Rendimiento a incorporar  #2 + #3 + #4 + #5 + #6 +7
        /// Observación Consistente/Inconsistente 
        /// Si #8 negative
        /// En caso que una fuente no tenga información de al menos una cuenta bancaria, la fuente no se podrá visualizar en ningún otro módulo de la plataforma.
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public async Task<Respuesta> ManagePerformanceAsync(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string action = ConstantCodigoAcciones.Tramitar_Rendimientos;
            int actionId = await GetActionIdAudit(action);
            string actionMesage = ConstantCommonMessages.Performances.TRAMITAR_RENDIMIENTOS;

            // Query Previous Month
            // Query Fuente by Valor Disponible > #8 Consistente
            // Query Fuente < Inconsistente

            // SELECT FROM Performance WHERE uploadedOrderId  =  uploadedOrderId
            // GROUP BY account numer, get month 

            // Check if is valid to prevent error 

            var collection = await _context.CarguePagosRendimientos.FindAsync(uploadedOrderId);

            List<PerformanceOrder> performanceOrders = new List<PerformanceOrder>();
            if (collection != null)
            {
                performanceOrders = JsonConvert.DeserializeObject<List<PerformanceOrder>>(collection.Json);
            }

            var accountNumbers = performanceOrders.Select(x => x.AccountNumber).Distinct();

            decimal valorAporteEnCuenta = 0;
            int registrosConsistentes = 0;
            foreach (var accountOrder in performanceOrders)
            {
                valorAporteEnCuenta = 0;
                var accounts = await _context.CuentaBancaria.SingleOrDefaultAsync(x => x.NumeroCuentaBanco == accountOrder.AccountNumber);

                if (accounts == null)
                {
                    new Exception("La cuenta asignada no existe");
                }
                var fundingSources = await _context.FuenteFinanciacion.
                    Where(r => !(bool)r.Eliminado && accounts.FuenteFinanciacionId == r.FuenteFinanciacionId).
                    Include(r => r.Aportante).ToListAsync();

                foreach (var fundingSource in fundingSources)
                {
                    fundingSource.ControlRecurso = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == fundingSource.FuenteFinanciacionId && !(bool)x.Eliminado).ToList();
                    foreach (var control in fundingSource.ControlRecurso)
                    {
                        control.FuenteFinanciacion = null; //
                    }

                    foreach (var element in fundingSource.ControlRecurso)
                    {
                        valorAporteEnCuenta += element.ValorConsignacion;
                    }
                }

                //_fundingSourceService.GetListFuentesFinanciacionshort();
                //_commonService.listaFuenteRecursos()
                // Rendimientos a incorporar

                accountOrder.PerformancesToAdd = accountOrder.GeneratedPerformances
                    - 0 // Rendimientos incorporados
                    - accountOrder.FinancialLienProvision
                    - accountOrder.BankCharges
                    - accountOrder.DiscountedCharge
                    - 0 // Visitas
                    ;
                accountOrder.Status = "Inconsistente";
                if (valorAporteEnCuenta >= accountOrder.PerformancesToAdd)
                {
                    accountOrder.Status = "Consistente";
                    registrosConsistentes += 1;
                }
            }

            response.Data = performanceOrders;
            response.IsSuccessful = true;
            response.Code = GeneralCodes.OperacionExitosa;
            
            var tramite = JsonConvert.SerializeObject(performanceOrders);
            try
            {
                await _context.Set<CarguePagosRendimientos>()
                  .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                  .UpdateAsync(o => new CarguePagosRendimientos()
                  {
                      FechaTramite = DateTime.Now,
                      TramiteJson = tramite,
                      RegistrosConsistentes = registrosConsistentes,
                      RegistrosInconsistentes = performanceOrders.Count - registrosConsistentes,
                  }); ;

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Code = GeneralCodes.Error;
            }


            response.Message = ""; // await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")


            return response;
        }

        /// <summary>
        /// Mark excel as 
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public Task<Respuesta> GetManagedPerformances(int uploadedOrderId, int statusType)
        {

            return null;
        }

        // Notify to Approve
        public async Task<Respuesta> NotifyRequestManagedPerformancesApproval(int uploadedOrderId, string author)
        {
            Respuesta response = new Respuesta();
            bool isMailSent = false;
#if !DEBUG
            isMailSent = true;
#endif
            int modifiedRows = -1;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Notificar_Solicitud_Aprobacion);
            string actionMesage = ConstantCommonMessages.Performances.NOTIFICAR_SOLICITUD_APROBACION;
            CarguePagosRendimientos uploadedPerformances = await _context.CarguePagosRendimientos
                    .Where(d => d.CargaPagosRendimientosId == uploadedOrderId).AsNoTracking().FirstOrDefaultAsync();

            if (uploadedPerformances == null || uploadedOrderId == 0)
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.GestionarRendimientos,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            try
            {
                Template temNotifyInconsistencies = await _commonService.GetTemplateById(
                                                            (int)enumeratorTemplate.NotificacionAprobacion);
                string inconsistencies = "";
                string fechaCargue = "";
                if (uploadedPerformances != null)
                {
                    inconsistencies = uploadedPerformances.RegistrosInconsistentes.ToString();
                    fechaCargue = Convert.ToDateTime(uploadedPerformances.FechaCargue).ToString("dd/MM/yyy");
                }
                // TODO Replace string
                string template = temNotifyInconsistencies.Contenido
                    .Replace("_LinkF_", _mailSettings.DominioFront).
                    Replace("[FECHA_CARGUE]", fechaCargue).
                    Replace("[INCONSISTENCIAS]", inconsistencies).
                    Replace("[URL]", _mailSettings.DominioFront);

                string subject = "";
#if !DEBUG
                isMailSent = this.SendMail(template, subject, EnumeratorPerfil.Fiduciaria);
#endif
                if (isMailSent)
                {
                    response.Code = ConstMessagesPerformances.ErrorEnviarCorreo;
                    response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                    return response;
                }

                
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                    .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                    .UpdateAsync(o => new CarguePagosRendimientos()
                    {
                            PendienteAprobacion = true,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = author
                    });


                response.IsSuccessful = isMailSent;
                response.Code = ConstMessagesPerformances.CorreoEnviado;
                string message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                response.Message = message;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.IsException = true;
                response.Code = GeneralCodes.Error;
                response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, ex.SubstringValid(_500));
            }

            return response;
        }


        /// <summary>
        /// Al guardar la información para los registros válidos, 
        /// el sistema deberá afectar los saldos de las cuentas asociadas 
        /// a las órdenes de giro especificadas en el archivo, de acuerdo 
        /// con los valores referenciados en cada una de ellas.
        /// </summary>
        /// <param name="typeFile"></param>
        /// <param name="listaCarguePagosRendimientos"></param>
        public void ProcessPayment(string typeFile, List<Dictionary<string, string>> listaCarguePagosRendimientos)
        {
            foreach (var pagoRendimiento in listaCarguePagosRendimientos)
            {
                if (typeFile == "Pagos")
                {
                    // cruce de pagos
                }
            }
        }

        public async Task<Respuesta> SetObservationPayments(string author, string observaciones, int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.OBSERVACIONES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Comentar_Pagos);
            int modifiedRows = 0;
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                response.Code = GeneralCodes.CamposVacios;
                response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage);
                return response;
            }

            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                      .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                      .UpdateAsync(o => new CarguePagosRendimientos()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = author,
                          Observaciones = observaciones
                      });

            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : GeneralCodes.EntradaInvalida;

            response.IsSuccessful = true;
            response.Code = codeResponse;
            response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage);

            return response;
        }


        /// <summary>
        /// el sistema notificará a la fiduciaria que se encuentra datos inconsistentes 
        /// y generará un archivo de Excel con la información reportada por la fiduciaria
        /// y la información calculada por el sistema, con el nombre “Inconsistentes”. 
        /// En la primera columna se presentará si el registro es inconsistente. 
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public async Task<Respuesta> NotifyEmailPerformanceInconsistencies(
            int uploadedOrderId, string author)
        {
            Respuesta response = new Respuesta();
            bool isMailSent = false;
#if !DEBUG
            isMailSent = true;
#endif
            int modifiedRows = -1;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Notificar_Inconsistencias);
            string actionMesage = ConstantCommonMessages.Performances.NOTIFICAR_INCONSISTENCIAS;
            CarguePagosRendimientos uploadedPerformances = null;

            uploadedPerformances = await _context.CarguePagosRendimientos
                    .Where(d => d.CargaPagosRendimientosId == uploadedOrderId).AsNoTracking().FirstOrDefaultAsync();

            if (uploadedPerformances == null || uploadedOrderId == 0)
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.GestionarRendimientos,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            try
            {
                Template temNotifyInconsistencies = await _commonService.GetTemplateById(
                                                            (int)enumeratorTemplate.NotificacionInconsistencias);                   
                string inconsistencies = "";
                string fechaCargue = "";
                if (uploadedPerformances != null)
                {
                    inconsistencies = uploadedPerformances.RegistrosInconsistentes.ToString();
                    fechaCargue = Convert.ToDateTime(uploadedPerformances.FechaCargue).ToString("dd/MM/yyy");
                }
                 // TODO Replace string
                string template = temNotifyInconsistencies.Contenido
                    .Replace("_LinkF_", _mailSettings.DominioFront).
                    Replace("[FECHA_CARGUE]", fechaCargue).
                    Replace("[INCONSISTENCIAS]", inconsistencies).
                    Replace("[URL]", _mailSettings.DominioFront);

                    string subject = "";
#if !DEBUG
                isMailSent = this.SendMail(template, subject, EnumeratorPerfil.Fiduciaria);
#endif
                if (isMailSent)
                {
                    response.Code = ConstMessagesPerformances.ErrorEnviarCorreo;
                    response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                    return response;
                }

                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                        .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                        .UpdateAsync(o => new CarguePagosRendimientos()
                        {
                            FechaTramite = DateTime.Now,
                            MostrarInconsistencias = true,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = author
                        });

                response.IsSuccessful = isMailSent;
                response.Code = ConstMessagesPerformances.CorreoEnviado;
                string message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                                
                response.Message = message;
            } 
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.IsException = true;
                response.Code = GeneralCodes.Error;
                response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, ex.SubstringValid(_500));
            }
            return response;
        }
    

        public async Task<Respuesta> GetInconsistencies(string author, int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.VER_INCONSISTENCIAS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Ver_Inconsistencias);
            string directory = CheckFileDownloadDirectory();
            var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == uploadedOrderId)
                            .Select( order => new
                            {
                                Estado = order.CargueValido,
                                ArchivoJson = order.TramiteJson
                            }).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(collection.ArchivoJson))
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            List<PerformanceOrder> list = JsonConvert.DeserializeObject<List<PerformanceOrder>>(collection.ArchivoJson);

            WriteCollectionToPath("Inconsistencias", directory, list);
            ////the path of the file
            string newfilePath = directory + "/" + "Inconsistencias" + "_rev.xlsx";

            response.Data = newfilePath;
            response.IsSuccessful = true;
            response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.GestionarRendimientos,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage);

            return response;

        }
    }

}
