using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using System.IO;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Internal;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class ValidateWeeklyProgressService : IValidateWeeklyProgressService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ValidateWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }
         
        public async Task<List<VVerificarValidarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal)
        {
            return await _context.VVerificarValidarSeguimientoSemanal.Where(r =>  r.EstadoSeguimientoSemanalCodigo  == "3").ToListAsync();
        }


    }
}
