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
        Task<DateTime> CalculardiasLaborales(int pDias, DateTime pFechaCalcular);

        Task<DateTime> CalculardiasLaboralesTranscurridos(int pDias, DateTime pFechaCalcular);        

        Task<string> EnumeradorComiteTecnico();

        Task<string> EnumeradorComiteFiduciario();

        Task<string> EnumeradorContratacion();
         
        Task<List<dynamic>> GetUsuarioByPerfil(int idPerfil);
  
        string GetNombreLocalizacionByLocalizacionId(string pLocalizacionId);
        string GetNombreDepartamentoByIdMunicipio(string pIdMunicipio);
        string GetNombreRegionByIdMunicipio(string pIdDepartamento);

        Task<List<MenuPerfil>> GetMenuByRol(int pUserId);
        Task<List<Perfil>> GetProfile();

        Task<Template> GetTemplateByTipo(string pTipo);

        Task<Template> GetTemplateById(int pId);

        Task<List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio);
        Task<Dominio> GetDominioByIdTipoDominio(int pIdTipoDominio);
        Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenuId, string pCodigo, int pAccionId, string pUsuario, string pObservaciones);

        Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId);
        Task<int> GetDominioIdByNombreDominioAndTipoDominio(string pNombre, int pTipoDominioId);

        Task<int> GetLocalizacionIdByName(string pNombre, string pIdDepartamento);

        Task<List<Localicacion>> GetListDepartamento();

        Task<List<Localicacion>> GetListMunicipioByIdDepartamento(string pIdDepartamento);
        Task<List<int>> GetListVigenciaAportes(string pYearVigente, bool yearSiguienteEsVigente);

        Task<int> getInstitucionEducativaIdByName(string pNombre);

        Task<string> GetDominioCodigoByNombreDominioAndTipoDominio(string pNombre, int pTipoDominioId);

        Task<int> getSedeInstitucionEducativaIdByNameAndInstitucionPadre(string pNombre, int pIdPadre);

        Task<int> getInstitucionEducativaIdByCodigoDane(int pCodigoDane);

        Task<Localizacion> GetLocalizacionByLocalizacionId(string pLocalizacionId);

        Task<ContratoPoliza> GetContratoPolizaByContratoId(int pContratoId);

        Task<ContratoPoliza> GetLastContratoPolizaByContratoId(int pContratoId);

        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<Contratista> GetContratistaByContratistaId(int pContratistaId);

        Task<Localizacion> GetDepartamentoByIdMunicipio(string pIdMunicipio);

        Task<List<Localicacion>> ListDepartamentoByRegionId(string idRegion);

        Task<List<Localicacion>> ListRegion();

        Task<Dominio> GetDominioByNombreDominioAndTipoDominio(string pCodigo, int pTipoDominioId);

        Task<List<InstitucionEducativaSede>> ListIntitucionEducativaByMunicipioId(string pIdMunicipio);

        Task<List<InstitucionEducativaSede>> ListSedeByInstitucionEducativaId(int pInstitucionEducativaCodigo);

        Task<string> GetNombreDominioByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId);

        Task<string> GetNombreDominioByDominioID(int pDominioID);

        Task<List<Localicacion>> GetListMunicipioByIdMunicipio(string idMunicipio);

        Task<List<Localicacion>> GetListDepartamentoByIdMunicipio(string idMunicipio);

        Task<InstitucionEducativaSede> GetInstitucionEducativaById(int InstitucionEducativaById);
        Task<List<Usuario>> GetUsuariosByPerfil(int pIdPerfil);
    }
}
