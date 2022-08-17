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

        #region Liquidacion
        public async Task<dynamic> GetInfoLiquidacionByContratoId(int pContratoId)
        {
            var Contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                   .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                                   .FirstOrDefaultAsync();



            var List = await _context.VRegistrarLiquidacionContrato.Where(r => r.ContratoId == pContratoId).ToListAsync();

            return new
            {
                NumeroContrato = Contrato.NumeroContrato,
                Contratista = Contrato.Contratacion.Contratista.Nombre,
                ListLiquidacion = List
            };

        }

        #endregion




        #region Procesos defensa Judicial

        public async Task<dynamic> GetInfoProcesosDefensaByContratoId(int pContratoId)
        {
            var Contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                                   .Include(r => r.Contratacion)
                                                   .ThenInclude(r => r.Contratista).FirstOrDefaultAsync();
            var ListDefensa = await _context.VFichaContratoDefensaJudicial.Where(r => r.ContratoId == pContratoId).ToListAsync();

            return new
            {
                NumeroContrato = Contrato.NumeroContrato,
                Contratista = Contrato.Contratacion.Contratista.Nombre,
                ListDefensaJudicial = ListDefensa
            };

        }
         
        #endregion

        #region Controversias 

        public async Task<dynamic> GetInfoControversiasByContratoId(int pContratoId)
        {

            var Contrato = await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                            .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                            .Include(n => n.ControversiaContractual)
                            .FirstOrDefaultAsync();

            List<Dominio> ListParametricas = _context.Dominio.Where(v => v.TipoDominioId == (int)EnumeratorTipoDominio.Tipos_Controversia
                                                                      || v.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia
                                                                   )
                                                             .ToList();

            List<dynamic> ListControversias = new List<dynamic>();

            foreach (var ControversiaContractual in Contrato.ControversiaContractual)
            {
                ListControversias.Add(new
                {
                    FechaSolicitud = ControversiaContractual.FechaSolicitud,
                    NumeroSolicitud = ControversiaContractual.NumeroSolicitud,
                    TipoControversia = ListParametricas.Find(c => c.Codigo == ControversiaContractual.TipoControversiaCodigo && c.TipoDominioId == (int)EnumeratorTipoDominio.Tipos_Controversia).Nombre,
                    EstadoControversia = ListParametricas.Find(c => c.Codigo == ControversiaContractual.EstadoCodigo && c.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia).Nombre,
                    UrlSoporte = ControversiaContractual.RutaSoporte
                });
            }

            return new
            {
                NumeroContrato = Contrato.NumeroContrato,
                Contratista = Contrato.Contratacion.Contratista.Nombre,
                ListControversias
            };
        }

        #endregion

        #region Novedades
        public async Task<dynamic> GetInfoNovedadesByContratoId(int pContratoId)
        {

            var Contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                        .Include(r => r.Contratacion).ThenInclude(r => r.Contratista)
                                        .Include(n => n.NovedadContractual).ThenInclude(c => c.NovedadContractualDescripcion)
                                        .FirstOrDefault();

            List<Dominio> ListParametricas = _context.Dominio.Where(v => v.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual
                                                                      || v.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Novedad_Contractual
                                                                   )
                                                             .ToList();

            List<dynamic> ListNovedades = new List<dynamic>();


            foreach (var NovedadContractual in Contrato.NovedadContractual)
            {
                foreach (var NovedadContractualDescripcion in NovedadContractual.NovedadContractualDescripcion)
                {
                    ListNovedades.Add(new
                    {
                        FechaSolicitud = NovedadContractual.FechaSolictud,
                        NumeroSolicitud = NovedadContractual.NumeroSolicitud,
                        TipoNovedad = ListParametricas.Find(c => c.Codigo == NovedadContractualDescripcion.TipoNovedadCodigo && c.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).Nombre,
                        EstadoNovedad = ListParametricas.Find(c => c.Codigo == NovedadContractual.EstadoCodigo && c.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Novedad_Contractual).Nombre,
                        UrlSoporte = NovedadContractual.UrlSoporte
                    });
                }
            }

            return new
            {
                NumeroContrato = Contrato.NumeroContrato,
                Contratista = Contrato.Contratacion.Contratista.Nombre,
                ListNovedades
            };

        }

        #endregion

        #region Polizas y Seguros 

        public async Task<dynamic> GetInfoPolizasSegurosByContratoId(int pContratoId)
        {

            List<VFichaContratoPolizasYactualizaciones> ListPolizas = _context.VFichaContratoPolizasYactualizaciones.Where(r => r.ContratoId == pContratoId).ToList();

            List<VFichaContratoPolizasYactualizaciones> ListPolizasOriginal = ListPolizas.Where(r => (bool)r.EsDrpOriginal).ToList();
            List<VFichaContratoPolizasYactualizaciones> ListPolizasACT = ListPolizas.Where(r => !(bool)r.EsDrpOriginal).ToList();


            VFichaContratoPolizasYactualizaciones Primera = ListPolizasOriginal.FirstOrDefault();

            List<dynamic> dyListPolizas = new List<dynamic>();

            foreach (var PolizasOriginal in ListPolizasOriginal)
            {
                dyListPolizas.Add(new
                {
                    NumeroPoliza = PolizasOriginal.PolizasSeguros,
                    VigenciaPoliza = PolizasOriginal.Vigencia,
                    VigenciaAmparo = PolizasOriginal.VigenciaAmparo,
                    ValorAmparo = PolizasOriginal.ValorAmparo
                });
            }


            List<dynamic> dyListPolizasACT = new List<dynamic>();

            foreach (var PolizasACT in ListPolizasACT)
            {
                VFichaContratoPolizasYactualizaciones Original = ListPolizasOriginal.Find(r => r.PolizasSeguros == PolizasACT.PolizasSeguros);

                dyListPolizasACT.Add(new
                {
                    NombrePoliza = PolizasACT.PolizasSeguros,

                    VigenciaPoliza = Original.Vigencia,
                    VigenciaPolizaActualizada = PolizasACT.Vigencia,

                    VigenciaAmparo = Original.VigenciaAmparo,
                    VigenciaAmparoActualizada = PolizasACT.VigenciaAmparo,

                    ValorAmparo = Original.ValorAmparo,
                    ValorAmparoActualizada = PolizasACT.ValorAmparo,
                });
            }

            return new
            {
                Primera.NumeroContrato,
                Primera.NombreContratista,
                Primera.TipoContrato,
                Primera.NumeroPoliza,
                Primera.NombreAseguradora,
                Primera.NumeroCertificado,
                Primera.FechaExpedicion,
                dyListPolizas,
                dyListPolizasACT
            };
        }


        #endregion

        #region Contratacion

        public async Task<dynamic> GetInfoContratacionByContratoId(int pContratoId)
        {
            var Contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId)
                                            .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto).ThenInclude(r => r.Proyecto)
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
                    TipoIntervencion = ListTipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre,
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
                TieneDistribucionTerritorioGrupo = procesosSeleccion.EsDistribucionGrupos,
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
                ContratacionId = contrato.Contratacion.ContratacionId,
                NumeroContrato = contrato.NumeroContrato,
                Contratista = contrato.Contratacion.Contratista.Nombre,
                InfoProyectos = infoProyectos,
                TipoContrato = contrato.Contratacion.TipoSolicitudCodigo ==
                ConstanCodigoTipoContratacionString.Obra ? (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra))
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria),
                Vigencia = contrato?.ContratoPoliza?.FirstOrDefault()?.PolizaGarantia?.FirstOrDefault()?.Vigencia
            };

            return new
            {
                Informacion = infoContrato,
                TieneResumen = _context.VFichaContratoProyectoDrp.Any(c => c.ContratoId == pContratoId),
                TieneSeleccion = _context.VFichaContratoTieneProcesosSeleccion.Any(r => r.ContratoId == pContratoId),
                TieneContratacion = contrato.Contratacion.ContratacionId > 0,
                TienePolizasSeguros = _context.VFichaContratoPolizasYactualizaciones.Any(r => r.ContratoId == pContratoId),
                TieneEjecucionFinanciera = true,
                TieneNovedades = _context.NovedadContractual.Any(c => c.ContratoId == pContratoId),
                TieneControversias = _context.ControversiaContractual.Any(c => c.ContratoId == pContratoId),
                TieneProcesosDefenzaJudicial = _context.VFichaContratoDefensaJudicial.Any(r => r.ContratoId == pContratoId),
                TieneLiquidacion = _context.VRegistrarLiquidacionContrato.Any(r => r.ContratoId == pContratoId),
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
