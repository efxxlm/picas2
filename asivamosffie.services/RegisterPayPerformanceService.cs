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

        public RegisterPayPerformanceService(IConverter converter,
                                                            devAsiVamosFFIEContext context,
                                                            ICommonService commonService,
                                                            IDocumentService documentService,
                                                            IRegisterPreContructionPhase1Service registerPreContructionPhase1Service,
                                                            IBudgetAvailabilityService budgetAvailabilityService)
        {
            _converter = converter;
            _context = context;
            _documentService = documentService;
            _commonService = commonService;
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
            _budgetAvailabilityService = budgetAvailabilityService;
        }
        #region loads
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
                                tieneError = ValidateFinancialPerformanceFile(worksheet, indexWorkSheetRow, carguePagosRendimiento, tieneError);

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

                        //processPaymentsPerformances(typeFile, listaCarguePagosRendimientos);
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

        private static bool ValidateFinancialPerformanceFile(ExcelWorksheet worksheet, int indexRow, Dictionary<string, string> carguePagosRendimiento, bool hasError)
        {
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
                indexCell += 1;
            }            

            return hasError;
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

        public async Task<string> DownloadPaymentOrderFile(string pNameFiles, string pFilePatch, string pUser)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var streams = new MemoryStream();
            using (var packages = new ExcelPackage(streams))
            {
                var workSheet = packages.Workbook.Worksheets.Add("Sheet1");
                var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == 1).Select(
                    x => new {
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
                    }).ToList();
                workSheet.Cells.LoadFromCollection(collection, true);
                packages.Save();
                //convert the excel package to a byte array
                byte[] bin = packages.GetAsByteArray();
                Stream stream = new MemoryStream(bin);
                //the path of the file
                string filePath = pFilePatch + "/" + pNameFiles + "_rev.xlsx";

                //write the file to the disk
                File.WriteAllBytes(filePath, bin);
                return filePath;
            }
            return "";
        }

        public async Task<List<dynamic>> getPaymentsPerformances(string typeFile)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePagosRendimientos> lista = _context.CarguePagosRendimientos.ToList();

            lista.Where(w=>w.TipoCargue == typeFile).ToList().ForEach(c =>
            {
                listaContrats.Add(new
                {
                    CargaPagosRendimientosId = c.CargaPagosRendimientosId,
                    NombreArchivo = c.NombreArchivo,
                    Json = c.Json,
                    Observaciones = c.Observaciones,
                    TotalRegistros = c.TotalRegistros,
                    RegistrosValidos = c.RegistrosValidos,
                    RegistrosInvalidos = c.RegistrosInvalidos,
                    EstadoCargue = c.EstadoCargue,
                    FechaCargue = c.FechaCargue
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
        public async Task<bool> setStatusPaymentPerformance(string cargaPagosRendimientosId, string uploadStatus)
        {
            CarguePagosRendimientos CarguePagosRendimientos = _context.CarguePagosRendimientos.Find(int.Parse(cargaPagosRendimientosId));
            CarguePagosRendimientos.EstadoCargue = uploadStatus;
            _context.Entry<CarguePagosRendimientos>(CarguePagosRendimientos).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

    }

}
