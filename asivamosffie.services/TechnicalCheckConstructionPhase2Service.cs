using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class TechnicalCheckConstructionPhase2Service : ITechnicalCheckConstructionPhase2Service
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirements;

        public TechnicalCheckConstructionPhase2Service(ITechnicalRequirementsConstructionPhaseService technicalRequirements, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _technicalRequirements = technicalRequirements;
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<dynamic>> GetContractsGrid(string pUsuarioId, string pTipoContrato)
        {
            List<dynamic> listaContrats = new List<dynamic>();
            List<VRequisitosTecnicosInicioConstruccion> lista = new List<VRequisitosTecnicosInicioConstruccion>();
            if (pTipoContrato == ConstanCodigoTipoContratacion.Obra.ToString())
                lista = await _context.VRequisitosTecnicosInicioConstruccion.Where(r => r.EstadoCodigo == "6" || r.EstadoCodigo == "7" || r.EstadoCodigo == "8" || r.EstadoCodigo == "9" || r.EstadoCodigo == "10").ToListAsync();
            else
                lista = await _context.VRequisitosTecnicosInicioConstruccion.Where(r => r.EstadoCodigo == "6" || r.EstadoCodigo == "7" || r.EstadoCodigo == "8" || r.EstadoCodigo == "9" || r.EstadoCodigo == "10" || r.EstadoCodigo == "11").ToListAsync();
           
            
            lista.Where(c => c.TipoContratoCodigo == pTipoContrato).ToList()
                .ForEach(c =>
                {
                    listaContrats.Add(new
                    {
                        ContratoId = c.ContratoId,
                        FechaAprobacion = c.FechaAprobacion,
                        NumeroContrato = c.NumeroContrato,
                        CantidadProyectosAsociados = c.CantidadProyectosAsociados,
                        CantidadProyectosRequisitosAprobados = c.CantidadProyectosRequisitosAprobados,
                        CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                        EstadoCodigo = c.EstadoCodigo,
                        EstadoNombre = c.EstadoNombre,
                        Existeregistro = c.ExisteRegistro,
                    });
                });
            return listaContrats;
        }


    }
}
