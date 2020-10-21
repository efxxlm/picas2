using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Enumerator;
using Z.EntityFramework.Plus;
using System.Globalization;

namespace asivamosffie.services
{
    public class CommonService : ICommonService
    {
        private readonly devAsiVamosFFIEContext _context;

        public CommonService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetUsuarioByPerfil(int idPerfil)
        {

            List<dynamic> ListaUsuario = new List<dynamic>();

            var ListUsuarios = await _context.UsuarioPerfil.Where(r => r.PerfilId == idPerfil && (bool)r.Activo).Select(r => r.Usuario).Where(r => !(bool)r.Eliminado).Distinct().OrderBy(r => r.Nombres).ToListAsync();

            foreach (var item in ListUsuarios)
            {
                ListaUsuario.Add(
                                    new
                                    {
                                        item.UsuarioId,
                                        item.Nombres,
                                        item.Apellidos
                                    }
                                );
            }
            return ListaUsuario;
        }

        public async Task<string> EnumeradorComiteTecnico()
        {
            int cantidadDeResgistros = _context.ComiteTecnico.Where( ct => ct.EsComiteFiduciario == false || ct.EsComiteFiduciario == false ).Count();
            string Nomeclatura = "CT_"; 
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo );
        }

        public async Task<string> EnumeradorComiteFiduciario()
        {
            int cantidadDeResgistros = _context.ComiteTecnico.Where( ct => ct.EsComiteFiduciario == true ).Count();
            string Nomeclatura = "CF_"; 
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo );
        }
        
        public async Task<string> EnumeradorContratacion()
        { 
            int cantidadDeResgistros =  _context.Contratacion.Count();
            string Nomeclatura = "PI_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }
         
        public async Task<List<MenuPerfil>> GetMenuByRol(int pUserId)
        {
            int IdPerfil = await _context.UsuarioPerfil.Where(r => r.UsuarioId == pUserId).Select(r => r.PerfilId).FirstOrDefaultAsync();
            return _context.MenuPerfil.Where(r => r.PerfilId == IdPerfil && (bool)r.Activo).IncludeFilter(r => r.Menu).ToList();
        }

        public string GetNombreDepartamentoByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idPadre = _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefault();
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(idPadre)).FirstOrDefault().Descripcion;
        }

        public string GetNombreRegionByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idDepartamento = _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefault();
            string idRegion = _context.Localizacion.Where(r => r.LocalizacionId.Equals(idDepartamento)).FirstOrDefault().IdPadre;
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(idRegion)).FirstOrDefault().Descripcion;
        }

        public async Task<List<Perfil>> GetProfile()
        {
            return await _context.Perfil.ToListAsync();
        }

        public async Task<List<Usuario>> GetUsuariosByPerfil(int pIdPerfil)
        {
            return await _context.UsuarioPerfil.Where(u => u.PerfilId == pIdPerfil)
                                                .Include(u => u.Usuario)
                                                .Select(s => s.Usuario).ToListAsync();
        }

        public async Task<Template> GetTemplateById(int pId)
        {
            return await _context.Template.Where(r => r.TemplateId == pId && (bool)r.Activo).FirstOrDefaultAsync();
        }

        public async Task<Template> GetTemplateByTipo(string ptipo)
        {
            return await _context.Template.Where(r => r.Tipo.Equals(ptipo) && (bool)r.Activo).FirstOrDefaultAsync();
        }

        public async Task<List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio)
        {
            //JMARTINEZ
            List<Dominio> listDominio = await _context.Dominio.Where(r => r.TipoDominioId == pIdTipoDominio && (bool)r.Activo).ToListAsync();
            //Vuelve todo mayuscula
            listDominio.ForEach(r => r.Nombre.ToUpper());
            return listDominio;
        }

        public async Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenu, string pCodigo, int pAccionId, string pUsuario, string pObservaciones)
        {
            var retorno = await _context.MensajesValidaciones.Where(r => (bool)r.Activo && r.MenuId == pMenu && r.Codigo.Equals(pCodigo)).FirstOrDefaultAsync();
            /*almaceno auditoria*/
            _context.Auditoria.Add(new Auditoria { AccionId = pAccionId, MensajesValidacionesId = retorno.MensajesValidacionesId, Usuario = pUsuario.ToUpper(), Observacion = pObservaciones.ToUpper(), Fecha = DateTime.Now });
            _context.SaveChanges();
            return retorno.Mensaje;
        }

        public async Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).Select(r => r.DominioId).FirstOrDefaultAsync();
        }



        public async Task<List<Localicacion>> GetListDepartamento()
        {
            
            return await _context.Localizacion.Where(r => r.Nivel == 1)
            .Select(x => new Localicacion
            {
                LocalizacionId = x.LocalizacionId,
                Descripcion = x.Descripcion.ToLower()//jflorez lo paso a min para usar en frontedn la clase capitalize
            }).ToListAsync();
        }

        public async Task<List<Localicacion>> GetListMunicipioByIdDepartamento(string pIdDepartamento)
        {            
            if (!string.IsNullOrEmpty(pIdDepartamento))
            {
                return await _context.Localizacion.Where(r => r.IdPadre.Equals(pIdDepartamento)).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()//jflorez lo cambio para usar en  fron la clase capitalize
                }).ToListAsync();
            }
            else
            {
                return await _context.Localizacion.Where(r => r.Nivel == 2).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()//jflorez lo cambio para usar en  fron la clase capitalize
                }).ToListAsync();
            }

        }

        public async Task<List<int>> GetListVigenciaAportes(string pYearVigente, bool yearSiguienteEsVigente)
        {
            try
            {
                List<int> YearVigencia = new List<int>();

                int intYearDesde = Int32.Parse(pYearVigente);
                int yearHasta = (yearSiguienteEsVigente) ? DateTime.Now.AddYears(1).Year : DateTime.Now.Year;

                for (int i = intYearDesde; i < yearHasta + 1; i++)
                {
                    YearVigencia.Add(i);
                }

                return YearVigencia;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetDominioIdByNombreDominioAndTipoDominio(string pNombreDominio, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Nombre.Trim().ToUpper().Equals(pNombreDominio.Trim().ToUpper()) && r.TipoDominioId == pTipoDominioId).Select(r => r.DominioId).FirstOrDefaultAsync();

        }

        public async Task<int> GetLocalizacionIdByName(string pNombre, string pIdDepartamento)
        {
            if (pIdDepartamento.Equals("0"))
                return Int32.Parse(await _context.Localizacion.Where(r => r.Nivel == 1 && r.Descripcion.Trim().ToUpper().Equals(pNombre.Trim().ToUpper())).Select(r => r.LocalizacionId).FirstOrDefaultAsync());
            else
                return Int32.Parse(await _context.Localizacion.Where(r => r.IdPadre.Contains(pIdDepartamento) && r.Nivel == 2 && r.Descripcion.Trim().ToUpper().Equals(pNombre.Trim().ToUpper())).Select(r => r.LocalizacionId).FirstOrDefaultAsync());
        }

        public async Task<int> getInstitucionEducativaIdByName(string pNombre)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.Nombre.ToUpper().Equals(pNombre.ToUpper())).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<string> GetDominioCodigoByNombreDominioAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Nombre.Trim().ToUpper().Equals(pCodigo.Trim().ToUpper()) && r.TipoDominioId == pTipoDominioId).Select(r => r.Codigo).FirstOrDefaultAsync();
        }

        public async Task<int> getSedeInstitucionEducativaIdByNameAndInstitucionPadre(string pNombre, int pIdPadre)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == pIdPadre && r.Nombre.Equals(pNombre)).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<int> getInstitucionEducativaIdByCodigoDane(int pCodigoDane)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.CodigoDane == pCodigoDane).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<Localizacion> GetLocalizacionByLocalizacionId(string pLocalizacionId)
        {
            return await _context.Localizacion.Where(r => r.LocalizacionId.Equals(pLocalizacionId)).FirstOrDefaultAsync();
        }

        public async Task<ContratoPoliza> GetContratoPolizaByContratoId(int pContratoId)
        {
            return await _context.ContratoPoliza.Where(r => r.ContratoId.Equals(pContratoId)).FirstOrDefaultAsync();
        }
        
        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            return await _context.Contratacion.Where(r => r.ContratacionId.Equals(pContratacionId)).FirstOrDefaultAsync();
        }
      
        public async Task<Contratista> GetContratistaByContratistaId(int pContratistaId)
        {
            return await _context.Contratista.Where(r => r.ContratistaId.Equals(pContratistaId)).FirstOrDefaultAsync();
        }
        public string GetNombreLocalizacionByLocalizacionId(string pLocalizacionId)
        {
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(pLocalizacionId)).Select(r => r.Descripcion).FirstOrDefault();

        }

        public async Task<Localizacion> GetDepartamentoByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idPadre = await _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefaultAsync();
            return await _context.Localizacion.Where(r => r.LocalizacionId.Equals(idPadre)).FirstOrDefaultAsync();
        }

        public async Task<List<Localicacion>> ListDepartamentoByRegionId(string pIdRegion)
        {
            if (!string.IsNullOrEmpty(pIdRegion) && !pIdRegion.Contains("7"))
            {
                return await _context.Localizacion.Where(r => r.IdPadre.Equals(pIdRegion)).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()
                }).ToListAsync();
            }
            else
            {
                return await _context.Localizacion.Where(r => r.Nivel == 1).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()
                }).ToListAsync();
            }
        }

        public async Task<List<Localicacion>> ListRegion()
        {
            return await _context.Localizacion.Where(r => r.Nivel == 3).Select(x => new Localicacion
            {
                LocalizacionId = x.LocalizacionId,
                Descripcion = x.Descripcion.ToLower()
            }).ToListAsync();
        }

        public async Task<Dominio> GetDominioByNombreDominioAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).FirstOrDefaultAsync();
        }



        public async Task<List<InstitucionEducativaSede>> ListIntitucionEducativaByMunicipioId(string pIdMunicipio)
        {
            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == null && r.LocalizacionIdMunicipio.Trim().Equals(pIdMunicipio.Trim())).ToListAsync();
        }

        public async Task<List<InstitucionEducativaSede>> ListSedeByInstitucionEducativaId(int pInstitucionEducativaCodigo)
        {
            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == pInstitucionEducativaCodigo).ToListAsync();
        }

        public async Task<string> GetNombreDominioByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            string strNombreDominio = await _context.Dominio.Where(r => r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).Select(r => r.Nombre).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(strNombreDominio))
            {
                return "Error Parametrica";
            }
            return strNombreDominio;
        }

        public async Task<string> GetNombreDominioByDominioID(int pDominioID)
        {
            return await _context.Dominio.Where(r => r.DominioId == pDominioID).Select(r => r.Nombre).FirstOrDefaultAsync();
        }
         
        public async Task<List<Localicacion>> GetListMunicipioByIdMunicipio(string idMunicipio)
        {
            var munactual = _context.Localizacion.Find(idMunicipio);
            return await _context.Localizacion.Where(r => r.Nivel == 2 && r.IdPadre == munactual.IdPadre)
             .Select(x => new Localicacion
             {
                 LocalizacionId = x.LocalizacionId,
                 Descripcion = x.Descripcion.ToLower(),//jflorez lo paso a min para usar en frontedn la clase capitalize
                 IdPadre = x.IdPadre
             }).ToListAsync();
        }



        public async Task<List<Localicacion>> GetListDepartamentoByIdMunicipio(string idMunicipio)
        {
            var munactual = _context.Localizacion.Find(idMunicipio);
            var depactual = _context.Localizacion.Find(munactual.IdPadre);
            //var regactual = _context.Localizacion.Find(depactual.IdPadre);
            return await _context.Localizacion.Where(r => r.Nivel == 1 && r.IdPadre == depactual.IdPadre)
             .Select(x => new Localicacion
             {
                 LocalizacionId = x.LocalizacionId,
                 Descripcion = x.Descripcion.ToLower(),//jflorez lo paso a min para usar en frontedn la clase capitalize
                 IdPadre = x.IdPadre
             }).ToListAsync();
        }
         
        public async Task<InstitucionEducativaSede> GetInstitucionEducativaById(int InstitucionEducativaById)
        {
            return await _context.InstitucionEducativaSede.FindAsync(InstitucionEducativaById);

            //int InstitucionEducativaSedeById = InstitucionEducativaById;
            //return _context.InstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == InstitucionEducativaById).FirstOrDefault();
        }

        //public async Task<InstitucionEducativaSede> GetInstitucionEducativaById(int InstitucionEducativaById)
        //{
        //    //return await _context.InstitucionEducativaSede.FindAsync(InstitucionEducativaById);
        //    return _context.InstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == InstitucionEducativaById).FirstOrDefault();
        //}

        

    }

}



