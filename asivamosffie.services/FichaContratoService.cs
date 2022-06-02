using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
namespace asivamosffie.services
{
    public class FichaContratoService : IFichaContratoService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public FichaContratoService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;

        }
        public async Task<dynamic> GetFlujoContratoByContratoId(int pContratoId)
        { 
            var contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                .Include(c => c.Contratacion)
                                                .ThenInclude(c => c.Contratista)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(c => c.InstitucionEducativa)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(c => c.Sede)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(c => c.Departamento)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto).ThenInclude(c => c.Municipio)  
                                                .FirstOrDefault();
                                                
            return new
            {
                Informacion = contrato,
                TieneResumen = true,
                TieneContratacion = true,
                TienePreparacion = true,
                TieneSeguimientoTecnico = true,
                TieneSeguimientoFinanciero = true,
                TieneEntrega = true,
            }; 
        }
        public async Task<dynamic> GetContratosByNumeroContrato(string pNumeroContrato)
        {

            List<VFichaContratoBusquedaContrato> LsitVFichaContratoBusquedaContrato = await _context.VFichaContratoBusquedaContrato
                                                                                        //.Where(r => r.NumeroContrato.ToUpper().Contains(pNumeroContrato.ToUpper()))
                                                                                        .ToListAsync();

            return LsitVFichaContratoBusquedaContrato.OrderByDescending(o => o.ContratoId).ToList();
        }
    }
}
