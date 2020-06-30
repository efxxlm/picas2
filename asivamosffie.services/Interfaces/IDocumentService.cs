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
        Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile , string pFilePatch , string pUsuarioCreo);
    }
}
