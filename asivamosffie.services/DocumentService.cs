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

        public DocumentService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        public async Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch)
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
                                    //Campos Obligatorios Validos
                                    if (
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) |
                                        !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                                    {

                                        TemporalProyecto temporalProyecto = new TemporalProyecto();
                                        //Auditoria
                                        temporalProyecto.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                        temporalProyecto.EsValido = false;
                                        temporalProyecto.FechaCreacion = DateTime.Now;
                                        temporalProyecto.UsuarioCreacion = "UsuarioCreacion@Yopmail.com";

                                        //Fecha sesion Junta 
                                        string strDateTime = (worksheet.Cells[i, 1].Text).ToString();
                                        if (!string.IsNullOrEmpty(strDateTime))
                                        {
                                            temporalProyecto.FechaSesionJunta = DateTime.ParseExact(strDateTime, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        archivoCarge.CantidadRegistrosValidos++;
                                    }
                                    else {

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
            respuesta.Message =  " Numero de veces" + CantidadIteraciones.ToString();
            return respuesta ;
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
