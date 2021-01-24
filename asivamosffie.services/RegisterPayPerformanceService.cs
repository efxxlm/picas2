using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Globalization;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using asivamosffie.services.Helpers.Constants;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Z.EntityFramework.Plus;
using DocumentFormat.OpenXml.Bibliography;

namespace asivamosffie.services
{
    public class RegisterPayPerformanceService : IRegisterPayPerformanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        public readonly IBudgetAvailabilityService _budgetAvailabilityService;
        public ISourceFundingService _fundingSourceService { get; }

        public RegisterPayPerformanceService(IConverter converter,
                                                            devAsiVamosFFIEContext context,
                                                            ISourceFundingService fundingSourceService,
                                                            ICommonService commonService,
                                                            IDocumentService documentService,
                                                            IRegisterPreContructionPhase1Service registerPreContructionPhase1Service,
                                                            IBudgetAvailabilityService budgetAvailabilityService)
        {
            _converter = converter;
            _context = context;
            _fundingSourceService = fundingSourceService;
            _documentService = documentService;
            _commonService = commonService;
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
            _budgetAvailabilityService = budgetAvailabilityService;
        }
        #region loads

        /// <summary>
        /// TODO El sistema debe registrar la trazabilidad almacenando el usuario,
        /// fecha, hora, acción y consulta realizada,
        /// con el fin de generar la auditoria. 
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="pUsuarioCreo"></param>
        /// <param name="typeFile"></param>
        /// <param name="saveSuccessProcess"></param>
        /// <returns></returns>
        public async Task<Respuesta> uploadFileToValidate(IFormFile pFile, string pUsuarioCreo, string typeFile, bool saveSuccessProcess)
        {
            int CantidadRegistrosInvalidos = 0;

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Registro_Pagos, (int)EnumeratorTipoDominio.Acciones);

            if (pFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);
                    List<Dictionary<string, string>> listaCarguePagosRendimientos = new List<Dictionary<string, string>>();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    // public ExcelRange this[int FromRow, int FromCol, int ToRow, int ToCol] { get; }
                    var columnAccounst = worksheet.Cells[2, 2, worksheet.Dimension.Rows, 2].Select( v=> v.Text).ToList<string>();

                    var bankAccounts = _context.CuentaBancaria.Where(
                        x => x.NumeroCuentaBanco != null && columnAccounst.Contains(x.NumeroCuentaBanco)).AsNoTracking();
                    // TODO add validation to prevent query a column in a  not existing position

                    //Controlar Registros
                    for (int indexWorkSheetRow = 2; indexWorkSheetRow <= worksheet.Dimension.Rows; indexWorkSheetRow++)
                    {
                        Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
                        bool tieneError = false;
                        try
                        {
                            if (typeFile == "Pagos")
                            {
                                tieneError = ValidatePaymentFile(worksheet, indexWorkSheetRow, carguePagosRendimiento, tieneError);
                            }
                            else if (typeFile == "Rendimientos")
                            {
                                var validatedValues = ValidateFinancialPerformanceFile(worksheet, indexWorkSheetRow, bankAccounts);

                                carguePagosRendimiento = validatedValues.list;
                                tieneError = validatedValues.hasError;
                            }

                            if (tieneError) {
                                CantidadRegistrosInvalidos++;
                                carguePagosRendimiento.Add("Estado", "Fallido");
                            } else {
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
                            NombreArchivo = pFile.FileName,
                            RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                            RegistrosInvalidos = CantidadRegistrosInvalidos,
                            TotalRegistros = cantidadRegistrosTotales,
                            TipoCargue = typeFile,
                            FechaCargue = DateTime.Now,
                            Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos)
                        };

                        _context.CarguePagosRendimientos.Add(CarguePagosRendimientos);
                        _context.SaveChanges();                        
                    }

                    if (CantidadRegistrosInvalidos == 0)
                    {
                        processPaymentsPerformances(typeFile, listaCarguePagosRendimientos);
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                    };
                }
            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                };
            }
        }

        private static bool ValidatePaymentFile(ExcelWorksheet worksheet, int i, Dictionary<string, string> carguePagosRendimiento, bool tieneError)
        {
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
            foreach (var rowFormat in performanceStructure)
            {
                string cellValue = worksheet.Cells[indexRow, indexCell].Text;
                carguePagosRendimiento.Add(rowFormat.Key, cellValue);
                if((rowFormat.Value == "Date")
                    && (string.IsNullOrEmpty(cellValue) || !DateTime.TryParseExact(cellValue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime fecha)))
                { 
                    hasError = true;
                }
                else if ((rowFormat.Value == "AlphaNum")
                    && (string.IsNullOrEmpty(cellValue) || cellValue.Length > 50))
                {
                        hasError = true;
                    
                }
                else if ((rowFormat.Value == "Money")
                    && (IsCellNullOrEmpty(cellValue) || IsNumericValidSize(cellValue)))
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
                            (activeAccount.Eliminado.HasValue && activeAccount.Eliminado.Value) ;
                    }
                }

                indexCell += 1;
            }

            // carguePagosRendimiento.Add("Estado", hasError ?  "Valido" : "Fallido");

            return (carguePagosRendimiento, hasError);
        }

        private static bool IsNumericValidSize(string value)
        {
            string cellValue = value.Replace(".", "").Replace("$", "");
            bool isValid = cellValue.Length > 20 || !decimal.TryParse(cellValue, out decimal respValNumber);
            return isValid;
        }

        private static bool IsCellNullOrEmpty(string cellValue)
        {
            return string.IsNullOrEmpty(cellValue);
        }

        public Respuesta DownloadPaymentPerformanceAsync(int uploadedOrderId)
        {
            // string pNameFiles, string pFilePatch, string pUser, int uploadOrderId
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var streams = new MemoryStream();
            //using (var packages = new ExcelPackage(streams))
            //{
            //    var workSheet = packages.Workbook.Worksheets.Add("Hoja 1");
             //Move to other method to shared ... 
                var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == uploadedOrderId)
                    .Select(
                    order => new {
                        NombreArchivo = order.NombreArchivo,
                        ArchivoJson = order.Json
                        //EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
                        //NombreArchivo = pFile.FileName,
                        //RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                        //RegistrosInvalidos = CantidadRegistrosInvalidos,
                        //TotalRegistros = cantidadRegistrosTotales,
                        //TipoCargue = typeFile,
                        //FechaCargue = DateTime.Now,
                        //Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos)


                        //Es_valido = x.EstaValidado,
                        //Tipo_de_proponente = x.TipoProponenteId,
                        //Nombre_del_proponente = x.NombreProponente,
                        //Numero_de_identificacion_del_proponente = x.NumeroIddentificacionProponente,
                        //Departamento_del_domicilio_del_proponente = x.Departamento,
                        //Municipio_del_domicilio_del_proponente = x.Minicipio,
                        //Direccion_del_proponente = x.Direccion,
                        //Telefono_del_proponente = x.Telefono,
                        //Correo_electronico_del_proponente = x.Correo,
                        //Nombre_de_la_Entidad = x.NombreEntidad,
                        //Numero_de_identificacion_Tributaria = x.IdentificacionTributaria,
                        //Nombre_del_Representante_legal = x.RepresentanteLegal,
                        //Cedula_del_representante = x.CedulaRepresentanteLegal,
                        //Departamento_del_domicilio_del_representante_legal = x.DepartamentoRl
                    }).FirstOrDefault();

            return new Respuesta
            {
                Data = collection,
                IsSuccessful = true,
                IsException = false,
                IsValidation = false,
                Code = GeneralCodes.OperacionExitosa,
                Message = "" // await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
            };

                //workSheet.Cells.LoadFromCollection(collection, true);
                //packages.Save();
                ////convert the excel package to a byte array
                //byte[] bin = packages.GetAsByteArray();
                //Stream stream = new MemoryStream(bin);
                ////the path of the file
                //string filePath = pFilePatch + "/" + pNameFiles + "_rev.xlsx";

                ////write the file to the disk
                //File.WriteAllBytes(filePath, bin);
                //return filePath;
            //}
            //return "";
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
                performanceOrders =
                    JsonConvert.DeserializeObject<List<PerformanceOrder>>(collection.Json);

            }

            // TODO ask if filter by date
            var accountOrders = performanceOrders.GroupBy(x => x.AccountNumber)
                .Select(accountOrders => new PerformanceOrder
                {
                    AccountNumber = accountOrders.Key,
                    ExemptResources = accountOrders.Sum(order => order.ExemptResources),
                    ExemptPerformances = accountOrders.Sum(order => order.ExemptPerformances),
                    ExemptBankCharges = accountOrders.Sum(order => order.ExemptBankCharges),
                    ExemptDiscountedCharge = accountOrders.Sum(order => order.ExemptDiscountedCharge),
                    LiableContributtions = accountOrders.Sum(order => order.LiableContributtions),
                    LiablePerformances = accountOrders.Sum(order => order.LiablePerformances),
                    LiableBankCharges = accountOrders.Sum(order => order.LiableBankCharges),
                    LiableDiscountedCharge = accountOrders.Sum(order => order.LiableDiscountedCharge)
                }).ToList();

            var accountNumbers = accountOrders.Select(x => x.AccountNumber);

            decimal valorAporteEnCuenta = 0;
            int registrosConsistentes = 0;
            foreach (var accountOrder in accountOrders)
            {
                valorAporteEnCuenta = 0;
                var accounts = await _context.CuentaBancaria.SingleOrDefaultAsync( x=> x.NumeroCuentaBanco == accountOrder.AccountNumber);

                if (accounts == null)
                {
                    new Exception("La cuenta asignada no existe");
                }
                var fundingSources = await _context.FuenteFinanciacion.
                    Where(r => !(bool)r.Eliminado && accounts.FuenteFinanciacionId ==  r.FuenteFinanciacionId).
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

            var respuesta = new Respuesta
            {
                Data = accountOrders,
                IsSuccessful = true,
                IsException = false,
                IsValidation = false,
                Code = GeneralCodes.OperacionExitosa,
                Message = "" // await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
            };
            var tramite = JsonConvert.SerializeObject(accountOrders);
            try
            {
                await _context.Set<CarguePagosRendimientos>()
                  .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                  .UpdateAsync(o => new CarguePagosRendimientos()
                  {
                      FechaTramite = DateTime.Now,
                      TramiteJson = tramite,
                      RegistrosConsistentes = registrosConsistentes,
                      RegistrosInconsistentes = accountOrders.Count - registrosConsistentes,
                  }); ;

            }
            catch (Exception ex)
            {

                throw ex;
            }
           

            // 

            return respuesta;
        }

        /// <summary>
        /// Mark excel as 
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public Task<Respuesta> getInconsistentPerformances(int uploadedOrderId)
        {

            return null;
        }

        // Notify to Approve
        public Task<Respuesta> NotifyToApprove(int uploadedOrderId) {

            return null;
        }

        // Notify to show Inconsistentes
        public Task<Respuesta> Notify(int uploadedOrderId)
        {

            return null;
        }

        public Task<Respuesta> ChangeStatusShowInconsistencies(int uploadOrderId)
        {
            return null;
        }

        /// <summary>
        /// TODO Add by status
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePagosRendimientos> lista =await _context.CarguePagosRendimientos.AsNoTracking().ToListAsync();

            lista.Where(w=>w.TipoCargue == typeFile && (string.IsNullOrEmpty(status)  || w.EstadoCargue == status) ).ToList().ForEach(c =>
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
                    c.EstadoCargue,
                    c.FechaCargue,
                    c.RegistrosInconsistentes,
                    c.RegistrosConsistentes //,
                    // c.FechaTramite
                });
            });

            return listaContrats;
        }

        public void processPaymentsPerformances(string typeFile, List<Dictionary<string, string>> listaCarguePagosRendimientos)
        {
            foreach (var pagoRendimiento in listaCarguePagosRendimientos)
            {
                if (typeFile == "Pagos")
                {
                    // cruce de pagos
                }
            }
        }

        public void setObservationPaymentsPerformances(string typeFile, string observaciones, string cargaPagosRendimientosId)
        {
            CarguePagosRendimientos CarguePagosRendimientos = _context.CarguePagosRendimientos.Find(int.Parse(cargaPagosRendimientosId));
            CarguePagosRendimientos.Observaciones = observaciones;
            _context.Entry<CarguePagosRendimientos>(CarguePagosRendimientos).State = EntityState.Modified;

            _context.SaveChanges();
        }


        #endregion

        #region crud
        /// <summary>
        /// TODO validar que este no tenga relaciones o información dependiente. 
        /// En caso de contar con alguna relación, el sistema debe informar al usuario
        /// por medio de un mensaje emergente “El registro tiene información que depende de él, no se puede eliminar”
        /// , y no deberá eliminar el registro.
        /// </summary>
        /// <param name="cargaPagosRendimientosId"></param>
        /// <param name="uploadStatus"></param>
        /// <returns></returns>
        public async Task<Respuesta> DeletePaymentPerformance(int uploadedOrderId)
        {
            int modifiedRows = -1;
            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                      .Where(order => order.CargaPagosRendimientosId == uploadedOrderId && !order.FechaTramite.HasValue)
                      .UpdateAsync(o => new CarguePagosRendimientos()
                      {
                      // FechaTramite Fecha Actualización on Update
                      EstadoCargue = "Eliminado",
                      }); ;

            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : GeneralCodes.EntradaInvalida;

            //  “El registro tiene información que depende de él, no se puede eliminar”

            return new Respuesta
            {
                Data = modifiedRows,
                IsSuccessful = modifiedRows > 0,
                IsException = false,
                IsValidation = false,
                Code = GeneralCodes.OperacionExitosa,
                Message = "" // await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Requisitos_Tecnicos_Construccion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
            };
        }
        #endregion

    }

}
