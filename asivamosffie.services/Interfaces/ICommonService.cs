using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface ICommonService
    {
        Task<List<Perfil>> GetProfile();

        Task <Template> GetTemplateByTipo(string pTipo);

        Task<Template> GetTemplateById(int pId);

        Task <List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio);
         
        Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenuId, string pCodigo);

        Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId);
    }
}
