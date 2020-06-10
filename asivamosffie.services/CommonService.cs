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
    }
}
