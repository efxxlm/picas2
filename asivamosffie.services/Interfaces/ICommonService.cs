using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface ICommonService
    {
        Task<List<Perfil>> GetProfile();

        Task<Template> GetTemplateByTipo(string pTipo);

        Task<Template> GetTemplateById(int pId);

        Task<List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio);

        Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenuId, string pCodigo, int pAccionId, string pUsuario, string pObservaciones);

        Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId);

        Task<int> GetDominioIdByNombreDominioAndTipoDominio(string pNombre, int pTipoDominioId);

        Task<int> GetLocalizacionIdByName(string pNombre, bool esDepartamento);

        Task<List<Localicacion>> GetListDepartamento();

        Task<List<Localicacion>> GetListMunicipioByIdDepartamento(string pIdDepartamento);

        Task<List<int>> GetListVigenciaAportes(string pYearVigente, bool yearSiguienteEsVigente);

        Task<int> getInstitucionEducativaIdByName(string pNombre);

        Task<string> GetDominioCodigoByNombreDominioAndTipoDominio(string pNombre, int pTipoDominioId);

    }
}