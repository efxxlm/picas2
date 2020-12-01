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

namespace asivamosffie.services
{
    public class RegisterWeeklyProgressService : IRegisterWeeklyProgressService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public RegisterWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<VRegistrarAvanceSemanal>> GetVRegistrarAvanceSemanal()
        {
            return await _context.VRegistrarAvanceSemanal.ToListAsync();
        }



        public async Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId)
        {

            return await _context.SeguimientoSemanal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId && !(bool)r.Eliminado && !(bool)r.RegistroCompleto)

                .Include(r=> r.SeguimientoDiario)

                .Include(r => r.SeguimientoSemanalAvanceFinanciero)
                  
                .Include(r => r.SeguimientoSemanalAvanceFisico)
                 
              


                .Include(r => r.SeguimientoSemanalPersonalObra)
        
                .Include(r => r.SeguimientoSemanalReporteActividad)

                .Include(r => r.SeguimientoSemanalRegistroFotografico)

                .Include(r => r.SeguimientoSemanalRegistrarComiteObra)

                .FirstOrDefaultAsync();
             
        }
    }
}
