using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
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
    }
}
