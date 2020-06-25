using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace asivamosffie.services
{
    public class CommonService : ICommonService
    {
        private readonly devAsiVamosFFIEContext _context;

        public CommonService(devAsiVamosFFIEContext context)
        {
            _context = context;            
        }
        public async Task<List<Perfil>> GetProfile()
        {
            return await _context.Perfil.ToListAsync();
        }

        public async Task<Template> GetTemplateById(int pId)
        {
            return await _context.Template.Where(r=> r.TemplateId==pId && (bool)r.Activo).FirstOrDefaultAsync();
        }

        public async Task<Template> GetTemplateByTipo(string ptipo)
        {
            return await _context.Template.Where(r => r.Tipo.Equals(ptipo) && (bool)r.Activo).FirstOrDefaultAsync();
        }
        public async Task<List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio)
        { 
            return await _context.Dominio.Where(r => r.TipoDominioId == pIdTipoDominio && (bool)r.Activo).ToListAsync(); 
        }

        public async Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenu, string pCodigo, int pAccionId, string pUsuario, string pObservaciones)
        {
            var retorno = await _context.MensajesValidaciones.Where(r => (bool)r.Activo && r.MenuId == pMenu && r.Codigo.Equals(pCodigo)).FirstOrDefaultAsync();
            /*almaceno auditoria*/
            _context.Auditoria.Add(new Auditoria{AccionId=pAccionId,MensajesValidacionesId=retorno.MensajesValidacionesId,Usuario=pUsuario, Observacion= pObservaciones });
            _context.SaveChangesAsync();
            return retorno.Mensaje;
        } 

        public async Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).Select(r=> r.DominioId).FirstOrDefaultAsync();
        }

        public async Task<List<Localicacion>> GetListDepartamento()
        { 
             return await _context.Localizacion.Where(r => r.Nivel == 1)
             .Select(x => new Localicacion
             {
                 LocalizacionId = x.LocalizacionId,
                 Descripcion = x.Descripcion
             }).ToListAsync();
         }
        public async Task<List<Localicacion>> GetListMunicipioByIdDepartamento(string pIdDepartamento)
        {
            if (!string.IsNullOrEmpty(pIdDepartamento))
            {
                return await _context.Localizacion.Where(r => r.IdPadre.Equals(pIdDepartamento)).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion
                }).ToListAsync();
            }
            else {
                return await _context.Localizacion.Where(r => r.Nivel == 2).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion
                }).ToListAsync();
            }
   
        }
    }
}
