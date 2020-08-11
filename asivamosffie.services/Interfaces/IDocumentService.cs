using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IDocumentService
    {

        Task<ArchivoCargue> getSaveFile(IFormFile pFile, string pFilePatch, int OrigenId);

        Task<List<ArchivoCargue>> GetListloadedDocuments(string pOrigenId = "1");

        Task<ArchivoCargue> GetArchivoCargueByName(string pNombre , string pUser);

    }
}