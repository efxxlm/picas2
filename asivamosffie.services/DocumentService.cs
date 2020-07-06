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
                ArchivoCargue archivoCargue = new ArchivoCargue();
                Guid g = Guid.NewGuid();
                archivoCargue.OrigenId = OrigenId;  
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

        public async  Task <List<ArchivoCargue>> GetListloadedDocuments () {

            return await _context.ArchivoCargue.Where(r => r.OrigenId.ToString().Equals(OrigenArchivoCargue.Proyecto) && (bool)r.Activo).ToListAsync();
        
        
        }

        public async Task<ArchivoCargue> GetArchivoCargueByName(string pNombre) {
             
            return await _context.ArchivoCargue.Where(r => r.Nombre.Contains(pNombre)).FirstOrDefaultAsync();
        } 
    }
}
