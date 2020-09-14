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
         
        public async Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch , int OrigenId)
        {
            try
            {
                Guid g = Guid.NewGuid();

                ArchivoCargue archivoCargue = new ArchivoCargue
                {
                    OrigenId = OrigenId,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    Ruta = pFilePatch,
                    Nombre = g.ToString(),
                    Tamano = pFile.Length.ToString()
                };
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
            catch (Exception )
            { 
                return new ArchivoCargue();
            }

        }

        public async  Task <List<ArchivoCargue>> GetListloadedDocuments(string pOrigenId = "1")
        {
            return await _context.ArchivoCargue.Where(r => r.OrigenId.ToString().Equals( pOrigenId ) && (bool)r.Activo).OrderByDescending(r=> r.ArchivoCargueId).OrderByDescending(r=> r.ArchivoCargueId).ToListAsync();
        }

        public async Task<ArchivoCargue> GetArchivoCargueByName(string pNombre , string pUser) {

            Respuesta respuesta = new Respuesta();
            ArchivoCargue archivoCargue = new ArchivoCargue();
            try
            { 
                archivoCargue = await _context.ArchivoCargue.Where(r => r.Nombre.Equals(pNombre)).FirstOrDefaultAsync();

                if (archivoCargue != null)
                {
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.DescargaExcelExitosa, (int)enumeratorAccion.DescargarExcelProyectos, pUser, "Se descargo el archivo")
                    };
                    return archivoCargue;
                }
                else {
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.ErrorDescargarArchivo, (int)enumeratorAccion.DescargarExcelProyectos, pUser, "No se encontro el archivo")
                    };
                    return archivoCargue;
                }
            }
            catch (Exception ex)
            {
                new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = ConstantMessagesCargueProyecto.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.CargueMasivoProyecto, ConstantMessagesCargueProyecto.ErrorDescargarArchivo, (int)enumeratorAccion.DescargarExcelProyectos, pUser, ex.ToString())
                };
                return archivoCargue;
            }
        } 
    }
}
