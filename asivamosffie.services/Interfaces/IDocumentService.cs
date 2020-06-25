using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
   public interface IDocumentService
    {
        Task<int> SetCargueMasivo(Usuario pUser, string filePath, byte[] v1, string v2, int idCategoria, string ipClient);
    }
}
