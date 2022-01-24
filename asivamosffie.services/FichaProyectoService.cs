﻿using asivamosffie.model.APIModels;
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
    public class FichaProyectoService : IFichaProyectoService
    {
        private readonly devAsiVamosFFIEContext _context;


        public FichaProyectoService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }


        #region Preparacion
        public async Task<dynamic> GetInfoPreparacionByContratacionProyectoId(int pContratacionProyecto)
        {

            return new
            {

                Informacion = _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ContratacionProyectoId == pContratacionProyecto)
                                                                             .Select(r => new
                                                                             {
                                                                                 r.LlaveMen,
                                                                                 r.InstitucionEducativa,
                                                                                 r.Sede,
                                                                                 r.Departamento,
                                                                                 r.Municipio,
                                                                                 r.TipoIntervencion
                                                                             }
                                                                                    )
                                                                            .FirstOrDefault(),

                Preconstruccion = GetGetInfoPreparacionPreConstruccionByContratacionProyectoId(pContratacionProyecto)

            };


        }

        public async Task<dynamic> GetGetInfoPreparacionPreConstruccionByContratacionProyectoId(int pContratacionProyecto)
        {

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pContratacionProyecto)
                                                                                     .Include(c => c.Contratacion).ThenInclude(c => c.Contrato)
                                                                                     .FirstOrDefault();


            return new
            {
                ActaSuscrita = contratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault()?.RutaActaSuscrita, 
                NumeroContrato = contratacionProyecto?.Contratacion?.Contrato?.FirstOrDefault()?.NumeroContrato,
                TablaPreconstruccion = _context.VFichaProyectoPreparacionPreconstruccion.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId
                                                                                                 && r.ContratacionId == contratacionProyecto.ContratacionId)
                                                                                        .ToList() 
            };
        }


        #endregion


        #region Busqueda
        public async Task<dynamic> GetFlujoProyectoByContratacionProyectoId(int pContratacionProyectoId)
        {
            return new
            {
                Informacion = _context.VFichaProyectoInfoContratacionProyecto.Where(x => x.ContratacionProyectoId == pContratacionProyectoId).FirstOrDefault(),
                TieneResumen = _context.VFichaProyectoTieneResumen.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
                TieneContratacion = _context.VFichaProyectoTieneContratacion.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
                TienePreparacion = _context.VFichaProyectoTienePreparacion.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
                TieneSeguimientoTecnico = _context.VFichaProyectoTieneSeguimientoTecnico.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
                TieneSeguimientoFinanciero = _context.VFichaProyectoTieneSeguimientoFinanciero.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
                TieneEntrega = _context.VFichaProyectoTieneEntrega.Any(cp => cp.ContratacionProyectoId == pContratacionProyectoId),
            };


        }

        public async Task<dynamic> GetVigencias()
        {
            return await _context.Cofinanciacion.Select(c => c.VigenciaCofinanciacionId)
                                                .OrderByDescending(c => c.Value)
                                                .ToListAsync();
        }
        public async Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen)
        {
            return await _context.VFichaProyectoBusquedaProyecto.Where(f => f.LlaveMen.ToUpper().Contains(pLlaveMen.ToUpper()))
                                                                .OrderByDescending(p => p.ProyectoId).ToListAsync();
        }
        public async Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoContrato, string pTipoIntervencion, int pVigencia)
        {

            List<VFichaProyectoBusquedaProyectoTabla> ListProyectos = await _context.VFichaProyectoBusquedaProyectoTabla.Where(p => p.ProyectoId == pProyectoId).ToListAsync();

            if (!string.IsNullOrEmpty(pTipoIntervencion))
                ListProyectos = ListProyectos.Where(p => p.CodigoTipoIntervencion == pTipoIntervencion).ToList();

            if (!string.IsNullOrEmpty(pTipoContrato))
                ListProyectos = ListProyectos.Where(p => p.CodigoTipoContrato == pTipoContrato).ToList();

            if (pVigencia > 0)
                ListProyectos = ListProyectos.Where(p => p.Vigencia == pVigencia).ToList();

            return ListProyectos;
        }

        #endregion
    }
}
