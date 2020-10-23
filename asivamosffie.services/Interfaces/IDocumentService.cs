using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IDocumentService
    {

        Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch, int OrigenId, int pRelacionId);

        Task<List<ArchivoCargue>> GetListloadedDocuments(string pOrigenId = "1");

        Task<ArchivoCargue> GetArchivoCargueByName(string pNombre , string pUser);

        Task<bool> SaveFileContratacion(IFormFile pFile, string pFilePatch, string pNameFile);
        Task<List<ArchivoCargue>> GetListloadedDocumentsByRelacionId(string pOrigenId, int pRelacionId);
        Task<string> DownloadOrdenElegibilidadFilesByName(string pNameFiles,string ruta, string pUser);
    }
}