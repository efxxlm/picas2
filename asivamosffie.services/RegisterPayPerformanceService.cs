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
        public async Task<Respuesta> uploadFileToValidate(IFormFile pFile,string pUsuarioCreo, string typeFile, bool saveSuccessProcess)
        {
            int CantidadRegistrosInvalidos = 0;
            
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Registro_Pagos, (int)EnumeratorTipoDominio.Acciones);

            if(pFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    
                    await pFile.CopyToAsync(stream);
                    List<Dictionary<string, string>> listaCarguePagosRendimientos = new List<Dictionary<string, string>>();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    //Controlar Registros
                    for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                    {
                        Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
                        bool tieneError = false;
                        try
                        {

                            if(typeFile == "Pagos")
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
                            }

                            if(tieneError)
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

                    if(CantidadRegistrosInvalidos > 0 || saveSuccessProcess)
                    {
                        CarguePagosRendimientos CarguePagosRendimientos = new CarguePagosRendimientos();
                        CarguePagosRendimientos.EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido";
                        CarguePagosRendimientos.NombreArchivo = pFile.FileName;
                        CarguePagosRendimientos.RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos;
                        CarguePagosRendimientos.RegistrosInvalidos = CantidadRegistrosInvalidos;
                        CarguePagosRendimientos.TotalRegistros = cantidadRegistrosTotales;
                        CarguePagosRendimientos.TipoCargue = typeFile;
                        CarguePagosRendimientos.FechaCargue = DateTime.Now;
                        CarguePagosRendimientos.Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos);

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

    }

}
