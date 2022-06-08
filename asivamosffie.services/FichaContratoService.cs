using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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


        #region Contratacion

        public async Task<dynamic> GetInfoContratacionByContratoId(int pContratoId)
        {
            var Contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto).ThenInclude(r=> r.Proyecto)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                            .FirstOrDefault();

            List<VFichaContratoProyectoDrp> vFichaContratoProyectoDrps = _context.VFichaContratoProyectoDrp.Where(c => c.ContratoId == pContratoId).ToList();

            List<Dominio> ListTipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<Localizacion> Localizacion = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();

            List<dynamic> VListaProyectos = new List<dynamic>();
            foreach (var ContratacionProyecto in Contrato.Contratacion.ContratacionProyecto)
            {
                Proyecto proyecto = ContratacionProyecto.Proyecto;

                InstitucionEducativaSede Sede = ListInstitucionEducativaSedes.Find(r => r.InstitucionEducativaSedeId == proyecto.SedeId);
                InstitucionEducativaSede InstitucionEducativa = ListInstitucionEducativaSedes.Find(r => r.InstitucionEducativaSedeId == Sede.PadreId);

                Localizacion Municipio = Localizacion.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio);
                Localizacion Departamento = Localizacion.Find(r => r.LocalizacionId == Municipio.IdPadre);


                VListaProyectos.Add(new 
                {
                   NumeroContrato = Contrato.NumeroContrato,
                   LlaveMen = proyecto.LlaveMen,
                   TipoIntervencion = ListTipoIntervencion.Where(r=> r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
                   Departamento = Departamento.Descripcion,
                   Municipio = Municipio.Descripcion,
                   InstitucionEducativa = InstitucionEducativa.Nombre,
                   Sede = Sede.Nombre
                }); 
            }


            return new 
            {
                NumeroContrato = Contrato.NumeroContrato,
                Contratista = Contrato.Contratacion.Contratista.Nombre,
                NumeroSolicitud = Contrato.Contratacion.NumeroSolicitud,
                FechaSolicitud = Contrato.Contratacion.FechaTramite,
                OpcionContratar = Contrato.Contratacion.TipoSolicitudCodigo == EnumeratorTipoSolicitudRequisitosPagosString.Contratos_de_obra ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria,
                ValorSolicitud = vFichaContratoProyectoDrps.Sum(c => c.ValorUso),
                UrlSoporte = Contrato.RutaDocumento,
                VListaProyectos
            };
        }

        #endregion

        #region ProcesosSeleccion
        public async Task<dynamic> GetInfoProcesosSeleccionByContratoId(int pContratoId)
        {
            var contrato = await _context.Contrato
                               .Where(c => c.ContratoId == pContratoId)
                               .Include(c => c.Contratacion)
                               .ThenInclude(c => c.Contratista)
                               .ThenInclude(r => r.ProcesoSeleccionProponente)
                               .ThenInclude(r => r.ProcesoSeleccion)
                               .ThenInclude(r => r.ProcesoSeleccionGrupo).FirstOrDefaultAsync();


            var procesosSeleccion = contrato.Contratacion.Contratista.ProcesoSeleccionProponente.ProcesoSeleccion;

            List<dynamic> GruposProcesoSeleccion = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Presupuesto_Proceso_de_Selección 
                                                                      || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion)
                                                             .ToList();

            int countCantidadGrupo = 1;

            foreach (var ProcesoSeleccionGrupo in contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.ProcesoSeleccion?.ProcesoSeleccionGrupo)
            {
                GruposProcesoSeleccion.Add(new
                {
                    GrupoNumero = 1,
                    NombreGrupo = ProcesoSeleccionGrupo.NombreGrupo,
                    TipoPresupuesto = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Presupuesto_Proceso_de_Selección 
                                                               && r.Codigo == ProcesoSeleccionGrupo.TipoPresupuestoCodigo)
                                                      .FirstOrDefault().Nombre,
                    Valor = ProcesoSeleccionGrupo.Valor,
                    PlazoMeses = ProcesoSeleccionGrupo.PlazoMeses

                });
                countCantidadGrupo++;
            }
            dynamic Contrato = new
            {
                NumeroContrato = contrato.NumeroContrato,
                Contratista = contrato.Contratacion.Contratista.Nombre,


                NumeroProceso = procesosSeleccion.NumeroProceso,
                FechaSolicitud = procesosSeleccion.FechaCreacion,
                TipoProceso = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Proceso_Seleccion
                                                               && r.Codigo == procesosSeleccion.TipoProcesoCodigo)
                                                      .FirstOrDefault().Nombre,
                Objeto = procesosSeleccion.Objeto,
                AlcanceParticular = procesosSeleccion.AlcanceParticular,
                FechaApertura = DateTime.Now,
                FechaEvaluacion = DateTime.Now,
                FechaAdjudicacion = DateTime.Now,
                FechaCierre = DateTime.Now,
                TieneDistribucionTerritorioGrupo  = procesosSeleccion.EsDistribucionGrupos,
                CuantosGrupos = procesosSeleccion.ProcesoSeleccionGrupo.Count,
                UrlSoporte = procesosSeleccion.UrlSoporteEvaluacion,
                  
                GruposProcesoSeleccion
            };


            return Contrato; 
        }
        #endregion

        #region Resumen

        public async Task<dynamic> GetInfoResumenByContratoId(int pContratoId)
        {
            List<dynamic> InfoContrato = new List<dynamic>();

            var contrato = await _context.Contrato
                                .Where(c => c.ContratoId == pContratoId)
                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto)
                                .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                                .FirstOrDefaultAsync();

            List<VFichaContratoProyectoDrp> vFichaContratoProyectoDrps = _context.VFichaContratoProyectoDrp.Where(c => c.ContratoId == pContratoId).ToList();
            string strEstadoContratacion = await _commonService.GetNombreDominioByCodigoAndTipoDominio(contrato.Contratacion.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud);

            foreach (var item in vFichaContratoProyectoDrps.Where(r => r.AportanteId > 0).ToList())
            {
                item.NombreAportante = GetNombreAportante(_context.CofinanciacionAportante.Find(item.AportanteId));
            }

            List<dynamic> ListProyectos = new List<dynamic>();
            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                List<VFichaContratoProyectoDrp> vFichaContratoProyectoDrp = vFichaContratoProyectoDrps.Where(c => c.ProyectoId == ContratacionProyecto.ProyectoId)
                                                                                                     .ToList();

                List<dynamic> fuentesUsos = new List<dynamic>();

                foreach (var ProyectoDrp in vFichaContratoProyectoDrp)
                {
                    FuenteFinanciacion fuenteFinanciacion = _context.FuenteFinanciacion.Find(ProyectoDrp.FuenteFinanciacionId);
                    string strNombreFuenteFinanciacion = await _commonService.GetNombreDominioByCodigoAndTipoDominio(fuenteFinanciacion.FuenteRecursosCodigo, (int)EnumeratorTipoDominio.Fuentes_de_financiacion);

                    fuentesUsos.Add(new
                    {
                        Aportante = ProyectoDrp.NombreAportante,
                        ValorAportante = ProyectoDrp.ValorUso,
                        Fuente = strNombreFuenteFinanciacion,
                        Uso = ProyectoDrp.Nombre,
                        ValorUso = ProyectoDrp.ValorUso
                    });
                }
                VFichaContratoProyectoDrp Proyecto = vFichaContratoProyectoDrp.FirstOrDefault();

                dynamic ProyectoInf = new
                {
                    LlaveMen = Proyecto.LlaveMen,
                    TipoIntervencion = Proyecto.TipoIntervencion,
                    Departamento = Proyecto.Departamento,
                    Municipio = Proyecto.Municipio,
                    InstitucionEducativaSede = Proyecto.InstitucionEducativa,
                    Sede = Proyecto.Sede,
                    FuentesUsos = fuentesUsos,
                    Inversion = vFichaContratoProyectoDrp.GroupBy(c => c.NumeroDrp)
                                                                .Select(s => new
                                                                {
                                                                    NumeroDrp = s.Key,
                                                                    Valor = s.Sum(c => c.ValorUso)
                                                                })
                                                                .ToList()
                };

                ListProyectos.Add(new
                {
                    Proyecto = ProyectoInf
                });
            }


            InfoContrato.Add(new
            {
                contrato = new
                {
                    NumeroContrato = contrato.NumeroContrato,
                    Contratista = contrato.Contratacion.Contratista.Nombre,
                    EstadoContrato = strEstadoContratacion,
                    ValorContrato = vFichaContratoProyectoDrps.Sum(c => c.ValorUso),
                    TipoContrato = contrato.Contratacion.TipoSolicitudCodigo ==
                ConstanCodigoTipoContratacionString.Obra ? (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra))
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria),
                    FechaSuscripcion = contrato.FechaAprobacionPoliza
                },
                ListProyectos
            });

            return InfoContrato;
        }


        #endregion

        #region Busqueda
        public async Task<dynamic> GetFlujoContratoByContratoId(int pContratoId)
        {
            var contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                .Include(r => r.ContratoPoliza).ThenInclude(c => c.PolizaGarantia)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                                                .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(r => r.Proyecto)
                                                .FirstOrDefault();


            List<dynamic> infoProyectos = new List<dynamic>();

            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                Proyecto proyecto = _context.Proyecto.Where(r => r.ProyectoId == ContratacionProyecto.ProyectoId)
                                                    .Include(c => c.InstitucionEducativa)
                                                    .Include(c => c.Sede)
                                                    .Include(c => c.LocalizacionIdMunicipioNavigation)
                                                    .FirstOrDefault();

                Localizacion Departamento = _context.Localizacion.Find(proyecto.LocalizacionIdMunicipioNavigation.IdPadre);
                infoProyectos.Add(new
                {
                    DepartamentoMunicipio = Departamento.Descripcion + " / " + ContratacionProyecto.Proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                    InstitucionEducativaSede = ContratacionProyecto.Proyecto.InstitucionEducativa.Nombre + " / " + ContratacionProyecto.Proyecto.Sede.Nombre,
                });
            }

            var infoContrato = new
            {
                ContratoId = pContratoId,
                NumeroContrato = contrato.NumeroContrato,
                Contratista = contrato.Contratacion.Contratista.Nombre,
                InfoProyectos = infoProyectos,
                TipoContrato = contrato.Contratacion.TipoSolicitudCodigo ==
                ConstanCodigoTipoContratacionString.Obra ? (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra))
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria),
                Vigencia = contrato.ContratoPoliza.FirstOrDefault().PolizaGarantia.FirstOrDefault().Vigencia
            };

            return new
            {
                Informacion = infoContrato,
                TieneResumen = true,
                TieneSeleccion = true,
                TieneContratacion = true,
                TienePolizasSeguros = true,
                TieneEjecucionFinanciera = true,
                TieneNovedades = true,
                TieneControversias = true,
                TieneProcesosDefenzaJudicial = true,
                TieneLiquidacion = true,
            };
        }
        public async Task<dynamic> GetContratosByNumeroContrato(string pNumeroContrato)
        {

            List<VFichaContratoBusquedaContrato> LsitVFichaContratoBusquedaContrato = await _context.VFichaContratoBusquedaContrato
                                                                                        //.Where(r => r.NumeroContrato.ToUpper().Contains(pNumeroContrato.ToUpper()))
                                                                                        .ToListAsync();

            return LsitVFichaContratoBusquedaContrato.OrderByDescending(o => o.ContratoId).ToList();
        }
        #endregion

        #region Utilidades

        private string GetNombreAportante(CofinanciacionAportante confinanciacion)
        {
            string nombreAportante = string.Empty;

            if (confinanciacion == null)
                return nombreAportante;

            if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                nombreAportante = ConstanStringTipoAportante.Ffie;
            }
            else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                nombreAportante = confinanciacion.NombreAportanteId == null
                    ? "Error" :
                    _context.Dominio.Find(confinanciacion.NombreAportanteId).Nombre;
            }
            else
            {
                if (confinanciacion.MunicipioId == null)
                {
                    nombreAportante = confinanciacion.DepartamentoId == null
                    ? "Error" :
                    "Gobernación " + _context.Localizacion.Find(confinanciacion.DepartamentoId).Descripcion;
                }
                else
                {
                    nombreAportante = confinanciacion.MunicipioId == null
                    ? "Error" :
                    "Alcaldía " + _context.Localizacion.Find(confinanciacion.MunicipioId).Descripcion;
                }
            }
            return nombreAportante;
        }


        #endregion
    }
}
