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
    public class FichaProyectoService : IFichaProyectoService
    {
        private readonly devAsiVamosFFIEContext _context;


        public FichaProyectoService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        #region Preparacion
        public async Task<dynamic> GetInfoPreparacionByProyectoId(int pProyectoId)
        {
            return new
            {

                Informacion = await _context.VFichaProyectoInfoContratacionProyecto.Where(r => r.ProyectoId == pProyectoId)
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
                                                                                .FirstOrDefaultAsync(),

                Preconstruccion = GetGetInfoPreparacionPreConstruccionByProyectoId(pProyectoId),

                Construccion = GetGetInfoPreparacionConstruccionByProyectoId(pProyectoId)
            };
        }

        public async Task<dynamic> GetGetInfoPreparacionConstruccionByProyectoId(int pProyectoId)
        {

            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            List<dynamic> Info = new List<dynamic>();


            foreach (var Contrato in ListContratosXProyecto)
            {
                ContratoConstruccion ContratoConstruccion = _context.ContratoConstruccion.Where(v => v.ContratoId == Contrato.ContratoId && v.ProyectoId == pProyectoId).FirstOrDefault();

                if (ContratoConstruccion != null)
                {
                    Info.Add(new
                    {
                        CodigoTipoContrato = Contrato.TipoContratoCodigo,
                        NombreTipoContrato = Contrato.NombreTipoContrato,
                        Diagnostico = GetDiagnosticoByContratoConstruccion(ContratoConstruccion),
                        PlanesYProgramas = GetPlantesYProgramasByContratoConstruccion(ContratoConstruccion)
                    });
                }
            }
            return Info;
        }

        private object GetPlantesYProgramasByContratoConstruccion(ContratoConstruccion pContratoConstruccion)
        {
            return new 
            {
                LicenciaVigente = pContratoConstruccion.PlanLicenciaVigente ?? false,

            };
        }

        private object GetDiagnosticoByContratoConstruccion(ContratoConstruccion pContratoConstruccion)
        {
            return new
            {
                CuentaConInformeDiagnosticoAprobado = pContratoConstruccion.EsInformeDiagnostico,
                UrlSoporte = pContratoConstruccion.RutaInforme,
                CostoDirecto = pContratoConstruccion.CostoDirecto,
                Administracion = pContratoConstruccion.Administracion,
                Imprevistos = pContratoConstruccion.Imprevistos,
                Utilidad = pContratoConstruccion.Utilidad,
                ValorTotalConstruccion = pContratoConstruccion.ValorTotalFaseConstruccion,
                TieneModificacionContractual = pContratoConstruccion.RequiereModificacionContractual,
                NumeroModificacion = pContratoConstruccion.NumeroSolicitudModificacion
            };

        }

        public async Task<dynamic> GetGetInfoPreparacionPreConstruccionByProyectoId(int pProyectoId)
        {

            List<VProyectosXcontrato> ListContratosXProyecto = _context.VProyectosXcontrato.Where(v => v.ProyectoId == pProyectoId).ToList();

            List<dynamic> Info = new List<dynamic>();

            foreach (var Contrato in ListContratosXProyecto)
            {
                Info.Add(new
                {
                    NombreTipoContrato = Contrato.NombreTipoContrato,
                    TipoContratoCodigo = Contrato.TipoContratoCodigo,
                    ActaSuscrita = Contrato.ActaSuscrita,
                    NumeroContrato = Contrato.NumeroContrato,
                    TablaPreconstruccion = _context.VFichaProyectoPreparacionPreconstruccion.Where(r => r.ProyectoId == pProyectoId
                                                                                                     && r.ContratacionId == Contrato.ContratacionId)
                                                                                            .ToList()
                });
            }
            return Info;
        }


        #endregion

        #region Busqueda
        public async Task<dynamic> GetFlujoProyectoByProyectoId(int pProyectoId)
        {
            return new
            {
                Informacion = _context.VFichaProyectoInfoContratacionProyecto.Where(x => x.ProyectoId == pProyectoId).FirstOrDefault(),
                TieneResumen = _context.VFichaProyectoTieneResumen.Any(cp => cp.ProyectoId == pProyectoId),
                TieneContratacion = _context.VFichaProyectoTieneContratacion.Any(cp => cp.ProyectoId == pProyectoId),
                TienePreparacion = _context.VFichaProyectoTienePreparacion.Any(cp => cp.ProyectoId == pProyectoId),
                TieneSeguimientoTecnico = _context.VFichaProyectoTieneSeguimientoTecnico.Any(cp => cp.ProyectoId == pProyectoId),
                TieneSeguimientoFinanciero = _context.VFichaProyectoTieneSeguimientoFinanciero.Any(cp => cp.ProyectoId == pProyectoId),
                TieneEntrega = _context.VFichaProyectoTieneEntrega.Any(cp => cp.ProyectoId == pProyectoId),
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
        public async Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoIntervencion, int pVigencia)
        {

            List<VFichaProyectoBusquedaProyectoTabla> ListProyectos = await _context.VFichaProyectoBusquedaProyectoTabla.Where(p => p.ProyectoId == pProyectoId).ToListAsync();

            if (!string.IsNullOrEmpty(pTipoIntervencion))
                ListProyectos = ListProyectos.Where(p => p.CodigoTipoIntervencion == pTipoIntervencion).ToList();
              
            if (pVigencia > 0)
                ListProyectos = ListProyectos.Where(p => p.Vigencia == pVigencia).ToList();

            return ListProyectos;
        }

        #endregion
    }
}
