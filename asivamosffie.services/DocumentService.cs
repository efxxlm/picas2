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
using System.IO;
using System.Text;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;

namespace asivamosffie.services
{
    public class DocumentService : IDocumentService
    {

        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public DocumentService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo)
        {
            int CantidadIteraciones = 1;
            Respuesta respuesta = new Respuesta();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                ArchivoCargue archivoCarge = await getSaveFile(pFile, pFilePatch);

                // if (!string.IsNullOrEmpty(archivoCarge.ArchivoCargueId.ToString()))
                if (archivoCarge != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await pFile.CopyToAsync(stream);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            //Controlar Registros
                            //Filas
                            for (int i = 2; i < worksheet.Dimension.Rows; i++)
                            {
                                CantidadIteraciones++;
                                try
                                {
                                    /* Columnas Obligatorias de excel
                                     2	3	4	5	6	7	8	10	11	12	13	14 28 29 30 31 32		
                                    Campos Obligatorios Validos   */
                                    if (
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 7].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 8].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 10].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 12].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 13].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 14].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 28].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 29].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 30].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 31].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 32].Text)
                                        )
                                    {

                                        TemporalProyecto temporalProyecto = new TemporalProyecto();
                                        //Auditoria
                                        temporalProyecto.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                        temporalProyecto.EsValido = false;
                                        temporalProyecto.FechaCreacion = DateTime.Now;
                                        temporalProyecto.UsuarioCreacion = pUsuarioCreo;

                                        // #1
                                        //Fecha sesion Junta 
                                        string strDateTime = (worksheet.Cells[i, 1].Text).ToString();
                                        if (!string.IsNullOrEmpty(strDateTime))
                                        {
                                            temporalProyecto.FechaSesionJunta = DateTime.Parse(strDateTime);
                                        }

                                        //#2
                                        //Número de acta de la junta 
                                        int intNumeroActaJunta = Int32.Parse(worksheet.Cells[i, 2].Text);
                                        temporalProyecto.NumeroActaJunta = intNumeroActaJunta;

                                        //#3
                                        // Tipo de Intervención 
                                        temporalProyecto.TipoIntervencionId = await _commonService.GetDominioIdByNombreDominioAndTipoDominio(worksheet.Cells[i, 3].Text, (int)EnumeratorTipoDominio.Tipo_de_Intervencion);

                                        //#4
                                        // Llave MEN  
                                        temporalProyecto.LlaveMen = worksheet.Cells[i, 4].Text;

                                        //#5
                                        //Región
                                         
                                        //#6
                                        //Departamento
                                        temporalProyecto.Departamento = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 6].Text, true);

                                        //#7
                                        //Municipio
                                        temporalProyecto.Municipio = await _commonService.GetLocalizacionIdByName(worksheet.Cells[i, 7].Text, false);

                                        //#8
                                        //Institución Educativa 
                                        //temporalProyecto.InstitucionEducativa = worksheet.Cells[i, 8].Text;










                                    }
                                    else
                                    {

                                        archivoCarge.CantidadRegistrosInvalidos++;
                                    }

                                }
                                catch (Exception)
                                {
                                    archivoCarge.CantidadRegistrosInvalidos++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    respuesta.Message = "NO Guardo ";
                }
            }
            catch (Exception ex)
            {

                respuesta.Message = ex.ToString();
            }
            respuesta.Message = " Numero de veces" + CantidadIteraciones.ToString();
            return respuesta;
        }


        public async Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch)
        {
            try
            {
                ArchivoCargue archivoCargue = new ArchivoCargue();
                Guid g = Guid.NewGuid();
                archivoCargue.OrigenId = 1;///Poner ID Para traer las cuestiones 
                archivoCargue.Activo = true;
                archivoCargue.FechaCreacion = DateTime.Now;
                archivoCargue.Ruta = pFilePatch;
                archivoCargue.Nombre = g.ToString();
                archivoCargue.Tamano = pFile.Length.ToString();
                if (!Directory.Exists(pFilePatch))
                {
                    Directory.CreateDirectory(pFilePatch);
                }
                var streamFile = new FileStream(pFilePatch + "/" + g.ToString() + "." + pFile.FileName.Split('.').Last(), FileMode.Create);
                using (streamFile)
                {
                    await pFile.CopyToAsync(streamFile);
                    _context.ArchivoCargue.Add(archivoCargue);
                    await _context.SaveChangesAsync();
                    return archivoCargue;
                }
            }
            catch (Exception e)
            {
                ArchivoCargue archivoCargue = new ArchivoCargue();
                return archivoCargue;
            }

        }

    }
}
