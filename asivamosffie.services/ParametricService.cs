using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Z.EntityFramework.Plus;
using System.Text.RegularExpressions;


namespace asivamosffie.services
{
    public class ParametricService : IParametricService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ParametricService(devAsiVamosFFIEContext devAsiVamosFFIEContext, ICommonService commonService)
        {
            _commonService = commonService;
            _context = devAsiVamosFFIEContext;
        } 
        public async Task<List<VParametricas>> GetParametricas()
        { 
            return await _context.VParametricas
                .OrderByDescending(p => p.TipoDominioId)
                .ToListAsync();
        } 
    }
}
