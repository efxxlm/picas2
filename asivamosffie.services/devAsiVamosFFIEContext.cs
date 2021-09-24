using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace asivamosffie.model.Models
{
    public partial class devAsiVamosFFIEContext : DbContext
    {
        public devAsiVamosFFIEContext()
        {
        }

        public devAsiVamosFFIEContext(DbContextOptions<devAsiVamosFFIEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActuacionSeguimiento> ActuacionSeguimiento { get; set; }
        public virtual DbSet<AjustePragramacionObservacion> AjustePragramacionObservacion { get; set; }
        public virtual DbSet<AjusteProgramacion> AjusteProgramacion { get; set; }
        public virtual DbSet<AjusteProgramacionFlujo> AjusteProgramacionFlujo { get; set; }
        public virtual DbSet<AjusteProgramacionObra> AjusteProgramacionObra { get; set; }
        public virtual DbSet<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual DbSet<ArchivoCargue> ArchivoCargue { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<BalanceFinanciero> BalanceFinanciero { get; set; }
        public virtual DbSet<BalanceFinancieroTraslado> BalanceFinancieroTraslado { get; set; }
        public virtual DbSet<BalanceFinancieroTrasladoValor> BalanceFinancieroTrasladoValor { get; set; }
        public virtual DbSet<CargueObservacion> CargueObservacion { get; set; }
        public virtual DbSet<CarguePago> CarguePago { get; set; }
        public virtual DbSet<CarguePagosRendimientos> CarguePagosRendimientos { get; set; }
        public virtual DbSet<Cofinanciacion> Cofinanciacion { get; set; }
        public virtual DbSet<CofinanciacionAportante> CofinanciacionAportante { get; set; }
        public virtual DbSet<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
        public virtual DbSet<ComiteTecnico> ComiteTecnico { get; set; }
        public virtual DbSet<ComponenteAportante> ComponenteAportante { get; set; }
        public virtual DbSet<ComponenteAportanteNovedad> ComponenteAportanteNovedad { get; set; }
        public virtual DbSet<ComponenteFuenteNovedad> ComponenteFuenteNovedad { get; set; }
        public virtual DbSet<ComponenteUso> ComponenteUso { get; set; }
        public virtual DbSet<ComponenteUsoNovedad> ComponenteUsoNovedad { get; set; }
        public virtual DbSet<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
        public virtual DbSet<ConceptoPagoUso> ConceptoPagoUso { get; set; }
        public virtual DbSet<ConstruccionCargue> ConstruccionCargue { get; set; }
        public virtual DbSet<ConstruccionObservacion> ConstruccionObservacion { get; set; }
        public virtual DbSet<ConstruccionPerfil> ConstruccionPerfil { get; set; }
        public virtual DbSet<ConstruccionPerfilNumeroRadicado> ConstruccionPerfilNumeroRadicado { get; set; }
        public virtual DbSet<ConstruccionPerfilObservacion> ConstruccionPerfilObservacion { get; set; }
        public virtual DbSet<Contratacion> Contratacion { get; set; }
        public virtual DbSet<ContratacionObservacion> ContratacionObservacion { get; set; }
        public virtual DbSet<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual DbSet<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual DbSet<Contratista> Contratista { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<ContratoConstruccion> ContratoConstruccion { get; set; }
        public virtual DbSet<ContratoObservacion> ContratoObservacion { get; set; }
        public virtual DbSet<ContratoPerfil> ContratoPerfil { get; set; }
        public virtual DbSet<ContratoPerfilNumeroRadicado> ContratoPerfilNumeroRadicado { get; set; }
        public virtual DbSet<ContratoPerfilObservacion> ContratoPerfilObservacion { get; set; }
        public virtual DbSet<ContratoPoliza> ContratoPoliza { get; set; }
        public virtual DbSet<ContratoPolizaActualizacion> ContratoPolizaActualizacion { get; set; }
        public virtual DbSet<ContratoPolizaActualizacionListaChequeo> ContratoPolizaActualizacionListaChequeo { get; set; }
        public virtual DbSet<ContratoPolizaActualizacionRevisionAprobacionObservacion> ContratoPolizaActualizacionRevisionAprobacionObservacion { get; set; }
        public virtual DbSet<ContratoPolizaActualizacionSeguro> ContratoPolizaActualizacionSeguro { get; set; }
        public virtual DbSet<ControlRecurso> ControlRecurso { get; set; }
        public virtual DbSet<ControversiaActuacion> ControversiaActuacion { get; set; }
        public virtual DbSet<ControversiaActuacionMesa> ControversiaActuacionMesa { get; set; }
        public virtual DbSet<ControversiaActuacionMesaSeguimiento> ControversiaActuacionMesaSeguimiento { get; set; }
        public virtual DbSet<ControversiaContractual> ControversiaContractual { get; set; }
        public virtual DbSet<ControversiaMotivo> ControversiaMotivo { get; set; }
        public virtual DbSet<CriterioCodigoTipoPagoCodigo> CriterioCodigoTipoPagoCodigo { get; set; }
        public virtual DbSet<CronogramaSeguimiento> CronogramaSeguimiento { get; set; }
        public virtual DbSet<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual DbSet<DefensaJudicial> DefensaJudicial { get; set; }
        public virtual DbSet<DefensaJudicialContratacionProyecto> DefensaJudicialContratacionProyecto { get; set; }
        public virtual DbSet<DefensaJudicialSeguimiento> DefensaJudicialSeguimiento { get; set; }
        public virtual DbSet<DemandadoConvocado> DemandadoConvocado { get; set; }
        public virtual DbSet<DemandanteConvocante> DemandanteConvocante { get; set; }
        public virtual DbSet<DevMenu> DevMenu { get; set; }
        public virtual DbSet<DisponibilidadPresupuestal> DisponibilidadPresupuestal { get; set; }
        public virtual DbSet<DisponibilidadPresupuestalObservacion> DisponibilidadPresupuestalObservacion { get; set; }
        public virtual DbSet<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
        public virtual DbSet<DocumentoApropiacion> DocumentoApropiacion { get; set; }
        public virtual DbSet<Dominio> Dominio { get; set; }
        public virtual DbSet<EnsayoLaboratorioMuestra> EnsayoLaboratorioMuestra { get; set; }
        public virtual DbSet<FaseComponenteUso> FaseComponenteUso { get; set; }
        public virtual DbSet<FichaEstudio> FichaEstudio { get; set; }
        public virtual DbSet<FlujoInversion> FlujoInversion { get; set; }
        public virtual DbSet<FormaPagoCriterioPago> FormaPagoCriterioPago { get; set; }
        public virtual DbSet<FormasPagoFase> FormasPagoFase { get; set; }
        public virtual DbSet<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual DbSet<GestionFuenteFinanciacion> GestionFuenteFinanciacion { get; set; }
        public virtual DbSet<GestionObraCalidadEnsayoLaboratorio> GestionObraCalidadEnsayoLaboratorio { get; set; }
        public virtual DbSet<GrupoMunicipios> GrupoMunicipios { get; set; }
        public virtual DbSet<IndicadorReporte> IndicadorReporte { get; set; }
        public virtual DbSet<InformeFinal> InformeFinal { get; set; }
        public virtual DbSet<InformeFinalAnexo> InformeFinalAnexo { get; set; }
        public virtual DbSet<InformeFinalInterventoria> InformeFinalInterventoria { get; set; }
        public virtual DbSet<InformeFinalInterventoriaObservaciones> InformeFinalInterventoriaObservaciones { get; set; }
        public virtual DbSet<InformeFinalListaChequeo> InformeFinalListaChequeo { get; set; }
        public virtual DbSet<InformeFinalObservaciones> InformeFinalObservaciones { get; set; }
        public virtual DbSet<InfraestructuraIntervenirProyecto> InfraestructuraIntervenirProyecto { get; set; }
        public virtual DbSet<InstitucionEducativaSede> InstitucionEducativaSede { get; set; }
        public virtual DbSet<LiquidacionContratacionObservacion> LiquidacionContratacionObservacion { get; set; }
        public virtual DbSet<ListaChequeo> ListaChequeo { get; set; }
        public virtual DbSet<ListaChequeoItem> ListaChequeoItem { get; set; }
        public virtual DbSet<ListaChequeoListaChequeoItem> ListaChequeoListaChequeoItem { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<ManejoMaterialesInsumos> ManejoMaterialesInsumos { get; set; }
        public virtual DbSet<ManejoMaterialesInsumosProveedor> ManejoMaterialesInsumosProveedor { get; set; }
        public virtual DbSet<ManejoOtro> ManejoOtro { get; set; }
        public virtual DbSet<ManejoResiduosConstruccionDemolicion> ManejoResiduosConstruccionDemolicion { get; set; }
        public virtual DbSet<ManejoResiduosConstruccionDemolicionGestor> ManejoResiduosConstruccionDemolicionGestor { get; set; }
        public virtual DbSet<ManejoResiduosPeligrososEspeciales> ManejoResiduosPeligrososEspeciales { get; set; }
        public virtual DbSet<MensajesValidaciones> MensajesValidaciones { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuPerfil> MenuPerfil { get; set; }
        public virtual DbSet<MesEjecucion> MesEjecucion { get; set; }
        public virtual DbSet<ModificacionContractual> ModificacionContractual { get; set; }
        public virtual DbSet<NovedadContractual> NovedadContractual { get; set; }
        public virtual DbSet<NovedadContractualAportante> NovedadContractualAportante { get; set; }
        public virtual DbSet<NovedadContractualClausula> NovedadContractualClausula { get; set; }
        public virtual DbSet<NovedadContractualDescripcion> NovedadContractualDescripcion { get; set; }
        public virtual DbSet<NovedadContractualDescripcionMotivo> NovedadContractualDescripcionMotivo { get; set; }
        public virtual DbSet<NovedadContractualObservaciones> NovedadContractualObservaciones { get; set; }
        public virtual DbSet<NovedadContractualRegistroPresupuestal> NovedadContractualRegistroPresupuestal { get; set; }
        public virtual DbSet<OrdenGiro> OrdenGiro { get; set; }
        public virtual DbSet<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
        public virtual DbSet<OrdenGiroDetalleDescuentoTecnica> OrdenGiroDetalleDescuentoTecnica { get; set; }
        public virtual DbSet<OrdenGiroDetalleDescuentoTecnicaAportante> OrdenGiroDetalleDescuentoTecnicaAportante { get; set; }
        public virtual DbSet<OrdenGiroDetalleEstrategiaPago> OrdenGiroDetalleEstrategiaPago { get; set; }
        public virtual DbSet<OrdenGiroDetalleObservacion> OrdenGiroDetalleObservacion { get; set; }
        public virtual DbSet<OrdenGiroDetalleTerceroCausacion> OrdenGiroDetalleTerceroCausacion { get; set; }
        public virtual DbSet<OrdenGiroDetalleTerceroCausacionAportante> OrdenGiroDetalleTerceroCausacionAportante { get; set; }
        public virtual DbSet<OrdenGiroDetalleTerceroCausacionDescuento> OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
        public virtual DbSet<OrdenGiroObservacion> OrdenGiroObservacion { get; set; }
        public virtual DbSet<OrdenGiroPago> OrdenGiroPago { get; set; }
        public virtual DbSet<OrdenGiroSoporte> OrdenGiroSoporte { get; set; }
        public virtual DbSet<OrdenGiroTercero> OrdenGiroTercero { get; set; }
        public virtual DbSet<OrdenGiroTerceroChequeGerencia> OrdenGiroTerceroChequeGerencia { get; set; }
        public virtual DbSet<OrdenGiroTerceroTransferenciaElectronica> OrdenGiroTerceroTransferenciaElectronica { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PlanesProgramasListaChequeoRespuesta> PlanesProgramasListaChequeoRespuesta { get; set; }
        public virtual DbSet<Plantilla> Plantilla { get; set; }
        public virtual DbSet<PlazoContratacion> PlazoContratacion { get; set; }
        public virtual DbSet<PolizaGarantia> PolizaGarantia { get; set; }
        public virtual DbSet<PolizaGarantiaActualizacion> PolizaGarantiaActualizacion { get; set; }
        public virtual DbSet<PolizaListaChequeo> PolizaListaChequeo { get; set; }
        public virtual DbSet<PolizaObservacion> PolizaObservacion { get; set; }
        public virtual DbSet<Predio> Predio { get; set; }
        public virtual DbSet<ProcesoSeleccion> ProcesoSeleccion { get; set; }
        public virtual DbSet<ProcesoSeleccionCotizacion> ProcesoSeleccionCotizacion { get; set; }
        public virtual DbSet<ProcesoSeleccionCronograma> ProcesoSeleccionCronograma { get; set; }
        public virtual DbSet<ProcesoSeleccionCronogramaMonitoreo> ProcesoSeleccionCronogramaMonitoreo { get; set; }
        public virtual DbSet<ProcesoSeleccionGrupo> ProcesoSeleccionGrupo { get; set; }
        public virtual DbSet<ProcesoSeleccionIntegrante> ProcesoSeleccionIntegrante { get; set; }
        public virtual DbSet<ProcesoSeleccionMonitoreo> ProcesoSeleccionMonitoreo { get; set; }
        public virtual DbSet<ProcesoSeleccionObservacion> ProcesoSeleccionObservacion { get; set; }
        public virtual DbSet<ProcesoSeleccionProponente> ProcesoSeleccionProponente { get; set; }
        public virtual DbSet<ProcesosContractualesObservacion> ProcesosContractualesObservacion { get; set; }
        public virtual DbSet<Programacion> Programacion { get; set; }
        public virtual DbSet<ProgramacionPersonalContrato> ProgramacionPersonalContrato { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<ProyectoAdministrativo> ProyectoAdministrativo { get; set; }
        public virtual DbSet<ProyectoAdministrativoAportante> ProyectoAdministrativoAportante { get; set; }
        public virtual DbSet<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual DbSet<ProyectoEntregaEtc> ProyectoEntregaEtc { get; set; }
        public virtual DbSet<ProyectoFuentes> ProyectoFuentes { get; set; }
        public virtual DbSet<ProyectoMonitoreoWeb> ProyectoMonitoreoWeb { get; set; }
        public virtual DbSet<ProyectoPredio> ProyectoPredio { get; set; }
        public virtual DbSet<ProyectoRequisitoTecnico> ProyectoRequisitoTecnico { get; set; }
        public virtual DbSet<RegistroPresupuestal> RegistroPresupuestal { get; set; }
        public virtual DbSet<RendimientosIncorporados> RendimientosIncorporados { get; set; }
        public virtual DbSet<RepresentanteEtcrecorrido> RepresentanteEtcrecorrido { get; set; }
        public virtual DbSet<RequisitoTecnicoRadicado> RequisitoTecnicoRadicado { get; set; }
        public virtual DbSet<SeguimientoActuacionDerivada> SeguimientoActuacionDerivada { get; set; }
        public virtual DbSet<SeguimientoDiario> SeguimientoDiario { get; set; }
        public virtual DbSet<SeguimientoDiarioObservaciones> SeguimientoDiarioObservaciones { get; set; }
        public virtual DbSet<SeguimientoSemanal> SeguimientoSemanal { get; set; }
        public virtual DbSet<SeguimientoSemanalAvanceFinanciero> SeguimientoSemanalAvanceFinanciero { get; set; }
        public virtual DbSet<SeguimientoSemanalAvanceFisico> SeguimientoSemanalAvanceFisico { get; set; }
        public virtual DbSet<SeguimientoSemanalAvanceFisicoProgramacion> SeguimientoSemanalAvanceFisicoProgramacion { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObra> SeguimientoSemanalGestionObra { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObraAlerta> SeguimientoSemanalGestionObraAlerta { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbiental { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObraCalidad> SeguimientoSemanalGestionObraCalidad { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObraSeguridadSalud> SeguimientoSemanalGestionObraSeguridadSalud { get; set; }
        public virtual DbSet<SeguimientoSemanalGestionObraSocial> SeguimientoSemanalGestionObraSocial { get; set; }
        public virtual DbSet<SeguimientoSemanalObservacion> SeguimientoSemanalObservacion { get; set; }
        public virtual DbSet<SeguimientoSemanalPersonalObra> SeguimientoSemanalPersonalObra { get; set; }
        public virtual DbSet<SeguimientoSemanalRegistrarComiteObra> SeguimientoSemanalRegistrarComiteObra { get; set; }
        public virtual DbSet<SeguimientoSemanalRegistroFotografico> SeguimientoSemanalRegistroFotografico { get; set; }
        public virtual DbSet<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividad { get; set; }
        public virtual DbSet<SeguimientoSemanalTemp> SeguimientoSemanalTemp { get; set; }
        public virtual DbSet<SeguridadSaludCausaAccidente> SeguridadSaludCausaAccidente { get; set; }
        public virtual DbSet<SesionComentario> SesionComentario { get; set; }
        public virtual DbSet<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
        public virtual DbSet<SesionComiteTecnicoCompromiso> SesionComiteTecnicoCompromiso { get; set; }
        public virtual DbSet<SesionComiteTema> SesionComiteTema { get; set; }
        public virtual DbSet<SesionInvitado> SesionInvitado { get; set; }
        public virtual DbSet<SesionParticipante> SesionParticipante { get; set; }
        public virtual DbSet<SesionParticipanteVoto> SesionParticipanteVoto { get; set; }
        public virtual DbSet<SesionSolicitudCompromiso> SesionSolicitudCompromiso { get; set; }
        public virtual DbSet<SesionSolicitudObservacionActualizacionCronograma> SesionSolicitudObservacionActualizacionCronograma { get; set; }
        public virtual DbSet<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual DbSet<SesionSolicitudVoto> SesionSolicitudVoto { get; set; }
        public virtual DbSet<SesionTemaVoto> SesionTemaVoto { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<SolicitudPago> SolicitudPago { get; set; }
        public virtual DbSet<SolicitudPagoCargarFormaPago> SolicitudPagoCargarFormaPago { get; set; }
        public virtual DbSet<SolicitudPagoExpensas> SolicitudPagoExpensas { get; set; }
        public virtual DbSet<SolicitudPagoFactura> SolicitudPagoFactura { get; set; }
        public virtual DbSet<SolicitudPagoFase> SolicitudPagoFase { get; set; }
        public virtual DbSet<SolicitudPagoFaseAmortizacion> SolicitudPagoFaseAmortizacion { get; set; }
        public virtual DbSet<SolicitudPagoFaseCriterio> SolicitudPagoFaseCriterio { get; set; }
        public virtual DbSet<SolicitudPagoFaseCriterioConceptoPago> SolicitudPagoFaseCriterioConceptoPago { get; set; }
        public virtual DbSet<SolicitudPagoFaseFacturaDescuento> SolicitudPagoFaseFacturaDescuento { get; set; }
        public virtual DbSet<SolicitudPagoListaChequeo> SolicitudPagoListaChequeo { get; set; }
        public virtual DbSet<SolicitudPagoListaChequeoRespuesta> SolicitudPagoListaChequeoRespuesta { get; set; }
        public virtual DbSet<SolicitudPagoObservacion> SolicitudPagoObservacion { get; set; }
        public virtual DbSet<SolicitudPagoOtrosCostosServicios> SolicitudPagoOtrosCostosServicios { get; set; }
        public virtual DbSet<SolicitudPagoRegistrarSolicitudPago> SolicitudPagoRegistrarSolicitudPago { get; set; }
        public virtual DbSet<SolicitudPagoSoporteSolicitud> SolicitudPagoSoporteSolicitud { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<TemaCompromiso> TemaCompromiso { get; set; }
        public virtual DbSet<TemaCompromisoSeguimiento> TemaCompromisoSeguimiento { get; set; }
        public virtual DbSet<Temp> Temp { get; set; }
        public virtual DbSet<TempFlujoInversion> TempFlujoInversion { get; set; }
        public virtual DbSet<TempOrdenLegibilidad> TempOrdenLegibilidad { get; set; }
        public virtual DbSet<TempProgramacion> TempProgramacion { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemporalProyecto> TemporalProyecto { get; set; }
        public virtual DbSet<TipoActividadGestionObra> TipoActividadGestionObra { get; set; }
        public virtual DbSet<TipoDominio> TipoDominio { get; set; }
        public virtual DbSet<TipoPagoConceptoPagoCriterio> TipoPagoConceptoPagoCriterio { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<VActualizacionPolizaYgarantias> VActualizacionPolizaYgarantias { get; set; }
        public virtual DbSet<VAjusteProgramacion> VAjusteProgramacion { get; set; }
        public virtual DbSet<VAportanteFuente> VAportanteFuente { get; set; }
        public virtual DbSet<VAportanteFuenteUso> VAportanteFuenteUso { get; set; }
        public virtual DbSet<VAportanteFuenteUsoProyecto> VAportanteFuenteUsoProyecto { get; set; }
        public virtual DbSet<VAportantesXcriterio> VAportantesXcriterio { get; set; }
        public virtual DbSet<VComponenteUsoNovedad> VComponenteUsoNovedad { get; set; }
        public virtual DbSet<VCompromisoSeguimiento> VCompromisoSeguimiento { get; set; }
        public virtual DbSet<VConceptosUsosXsolicitudPagoId> VConceptosUsosXsolicitudPagoId { get; set; }
        public virtual DbSet<VConfinanciacionReporte> VConfinanciacionReporte { get; set; }
        public virtual DbSet<VContratacionProyectoSolicitudLiquidacion> VContratacionProyectoSolicitudLiquidacion { get; set; }
        public virtual DbSet<VContratistaReporteHist> VContratistaReporteHist { get; set; }
        public virtual DbSet<VContratoPagosRealizados> VContratoPagosRealizados { get; set; }
        public virtual DbSet<VContratosXcontratacionProyecto> VContratosXcontratacionProyecto { get; set; }
        public virtual DbSet<VCuentaBancariaPago> VCuentaBancariaPago { get; set; }
        public virtual DbSet<VDefensaJudicialContratacionProyecto> VDefensaJudicialContratacionProyecto { get; set; }
        public virtual DbSet<VDescuentoTecnicaXordenGiro> VDescuentoTecnicaXordenGiro { get; set; }
        public virtual DbSet<VDescuentosFinancieraOdgxFuenteFinanciacionXaportante> VDescuentosFinancieraOdgxFuenteFinanciacionXaportante { get; set; }
        public virtual DbSet<VDescuentosOdgxFuenteFinanciacionXaportante> VDescuentosOdgxFuenteFinanciacionXaportante { get; set; }
        public virtual DbSet<VDescuentosXordenGiro> VDescuentosXordenGiro { get; set; }
        public virtual DbSet<VDescuentosXordenGiroAportante> VDescuentosXordenGiroAportante { get; set; }
        public virtual DbSet<VDescuentosXordenGiroXproyectoXaportanteXconcepto> VDescuentosXordenGiroXproyectoXaportanteXconcepto { get; set; }
        public virtual DbSet<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso { get; set; }
        public virtual DbSet<VDescuentosXordenGiroXproyectoXfaseXaportanteXconcepto> VDescuentosXordenGiroXproyectoXfaseXaportanteXconcepto { get; set; }
        public virtual DbSet<VDisponibilidadPresupuestal> VDisponibilidadPresupuestal { get; set; }
        public virtual DbSet<VDominio> VDominio { get; set; }
        public virtual DbSet<VDrpNovedadXfaseContratacionId> VDrpNovedadXfaseContratacionId { get; set; }
        public virtual DbSet<VDrpXcontratacionXproyectoXaportante> VDrpXcontratacionXproyectoXaportante { get; set; }
        public virtual DbSet<VDrpXcontratacionXproyectoXaportanteXfaseXcriterioXconceptoXusos> VDrpXcontratacionXproyectoXaportanteXfaseXcriterioXconceptoXusos { get; set; }
        public virtual DbSet<VDrpXcontratacionXproyectoXfaseXconceptoXusos> VDrpXcontratacionXproyectoXfaseXconceptoXusos { get; set; }
        public virtual DbSet<VDrpXcontratoXaportante> VDrpXcontratoXaportante { get; set; }
        public virtual DbSet<VDrpXfaseContratacionId> VDrpXfaseContratacionId { get; set; }
        public virtual DbSet<VDrpXfaseXcontratacionIdXnovedad> VDrpXfaseXcontratacionIdXnovedad { get; set; }
        public virtual DbSet<VDrpXproyectoXusos> VDrpXproyectoXusos { get; set; }
        public virtual DbSet<VEjecucionFinancieraXcontrato> VEjecucionFinancieraXcontrato { get; set; }
        public virtual DbSet<VEjecucionFinancieraXproyecto> VEjecucionFinancieraXproyecto { get; set; }
        public virtual DbSet<VEjecucionPresupuestalXproyecto> VEjecucionPresupuestalXproyecto { get; set; }
        public virtual DbSet<VFacturadoOdgXcontratacionXproyectoXfaseXconceptoXaportante> VFacturadoOdgXcontratacionXproyectoXfaseXconceptoXaportante { get; set; }
        public virtual DbSet<VFacturadoXodgXcontratacionXproyectoXaportanteXfaseXconcepXuso> VFacturadoXodgXcontratacionXproyectoXaportanteXfaseXconcepXuso { get; set; }
        public virtual DbSet<VFuentesUsoXcontratoId> VFuentesUsoXcontratoId { get; set; }
        public virtual DbSet<VFuentesUsoXcontratoIdXproyecto> VFuentesUsoXcontratoIdXproyecto { get; set; }
        public virtual DbSet<VGestionarGarantiasPolizas> VGestionarGarantiasPolizas { get; set; }
        public virtual DbSet<VListCompromisosComiteTecnico> VListCompromisosComiteTecnico { get; set; }
        public virtual DbSet<VListCompromisosTemas> VListCompromisosTemas { get; set; }
        public virtual DbSet<VListaContratacionModificacionContractual> VListaContratacionModificacionContractual { get; set; }
        public virtual DbSet<VListaProyectos> VListaProyectos { get; set; }
        public virtual DbSet<VNombreCuentaXodgXaportanteXconcepto> VNombreCuentaXodgXaportanteXconcepto { get; set; }
        public virtual DbSet<VNovedadContractual> VNovedadContractual { get; set; }
        public virtual DbSet<VNovedadContractualReporteHist> VNovedadContractualReporteHist { get; set; }
        public virtual DbSet<VOrdenGiro> VOrdenGiro { get; set; }
        public virtual DbSet<VOrdenGiroPagosXusoAportante> VOrdenGiroPagosXusoAportante { get; set; }
        public virtual DbSet<VOrdenGiroPagosXusoAportanteXproyecto> VOrdenGiroPagosXusoAportanteXproyecto { get; set; }
        public virtual DbSet<VOrdenGiroXproyecto> VOrdenGiroXproyecto { get; set; }
        public virtual DbSet<VPagosSolicitudXcontratacionXproyectoXuso> VPagosSolicitudXcontratacionXproyectoXuso { get; set; }
        public virtual DbSet<VPagosSolicitudXodgXcontratacionXproyectoXuso> VPagosSolicitudXodgXcontratacionXproyectoXuso { get; set; }
        public virtual DbSet<VParametricas> VParametricas { get; set; }
        public virtual DbSet<VPermisosMenus> VPermisosMenus { get; set; }
        public virtual DbSet<VPlantillaOrdenGiro> VPlantillaOrdenGiro { get; set; }
        public virtual DbSet<VPlantillaOrdenGiroUsos> VPlantillaOrdenGiroUsos { get; set; }
        public virtual DbSet<VProcesoSeleccionIntegrante> VProcesoSeleccionIntegrante { get; set; }
        public virtual DbSet<VProcesoSeleccionReporteHist> VProcesoSeleccionReporteHist { get; set; }
        public virtual DbSet<VProgramacionBySeguimientoSemanal> VProgramacionBySeguimientoSemanal { get; set; }
        public virtual DbSet<VProyectoReporteHist> VProyectoReporteHist { get; set; }
        public virtual DbSet<VProyectosBalance> VProyectosBalance { get; set; }
        public virtual DbSet<VProyectosCierre> VProyectosCierre { get; set; }
        public virtual DbSet<VProyectosXcontrato> VProyectosXcontrato { get; set; }
        public virtual DbSet<VRegistrarAvanceSemanal> VRegistrarAvanceSemanal { get; set; }
        public virtual DbSet<VRegistrarAvanceSemanalCompletos> VRegistrarAvanceSemanalCompletos { get; set; }
        public virtual DbSet<VRegistrarAvanceSemanalNew> VRegistrarAvanceSemanalNew { get; set; }
        public virtual DbSet<VRegistrarFase1> VRegistrarFase1 { get; set; }
        public virtual DbSet<VRegistrarLiquidacionContrato> VRegistrarLiquidacionContrato { get; set; }
        public virtual DbSet<VRegistrarPersonalObra> VRegistrarPersonalObra { get; set; }
        public virtual DbSet<VRendimientodXcuentaBancaria> VRendimientodXcuentaBancaria { get; set; }
        public virtual DbSet<VReporteProyectos> VReporteProyectos { get; set; }
        public virtual DbSet<VRequisitosTecnicosConstruccionAprobar> VRequisitosTecnicosConstruccionAprobar { get; set; }
        public virtual DbSet<VRequisitosTecnicosInicioConstruccion> VRequisitosTecnicosInicioConstruccion { get; set; }
        public virtual DbSet<VRequisitosTecnicosPreconstruccion> VRequisitosTecnicosPreconstruccion { get; set; }
        public virtual DbSet<VRpsPorContratacion> VRpsPorContratacion { get; set; }
        public virtual DbSet<VSaldoPresupuestalXcontrato> VSaldoPresupuestalXcontrato { get; set; }
        public virtual DbSet<VSaldoPresupuestalXproyecto> VSaldoPresupuestalXproyecto { get; set; }
        public virtual DbSet<VSaldosFuenteXaportanteId> VSaldosFuenteXaportanteId { get; set; }
        public virtual DbSet<VSaldosFuenteXaportanteIdValidar> VSaldosFuenteXaportanteIdValidar { get; set; }
        public virtual DbSet<VSeguimientoSemanalRegistrar> VSeguimientoSemanalRegistrar { get; set; }
        public virtual DbSet<VSesionParticipante> VSesionParticipante { get; set; }
        public virtual DbSet<VSetHistDefensaJudicial> VSetHistDefensaJudicial { get; set; }
        public virtual DbSet<VSetHistDefensaJudicialContratacionProyecto> VSetHistDefensaJudicialContratacionProyecto { get; set; }
        public virtual DbSet<VSetHistProyectoAportante> VSetHistProyectoAportante { get; set; }
        public virtual DbSet<VSolicitudPago> VSolicitudPago { get; set; }
        public virtual DbSet<VTablaOdgDescuento> VTablaOdgDescuento { get; set; }
        public virtual DbSet<VTablaOdgFacturado> VTablaOdgFacturado { get; set; }
        public virtual DbSet<VTablaOdgOtroDescuento> VTablaOdgOtroDescuento { get; set; }
        public virtual DbSet<VTotalComprometidoXcontratacionProyectoTipoSolicitud> VTotalComprometidoXcontratacionProyectoTipoSolicitud { get; set; }
        public virtual DbSet<VUbicacionXproyecto> VUbicacionXproyecto { get; set; }
        public virtual DbSet<VUsosXsolicitudPago> VUsosXsolicitudPago { get; set; }
        public virtual DbSet<VUsuarioPerfil> VUsuarioPerfil { get; set; }
        public virtual DbSet<VUsuarioRol> VUsuarioRol { get; set; }
        public virtual DbSet<VValidarSeguimientoSemanal> VValidarSeguimientoSemanal { get; set; }
        public virtual DbSet<VValorConstruccionXproyectoContrato> VValorConstruccionXproyectoContrato { get; set; }
        public virtual DbSet<VValorFacturadoContrato> VValorFacturadoContrato { get; set; }
        public virtual DbSet<VValorFacturadoContratoXproyecto> VValorFacturadoContratoXproyecto { get; set; }
        public virtual DbSet<VValorFacturadoContratoXproyectoXuso> VValorFacturadoContratoXproyectoXuso { get; set; }
        public virtual DbSet<VValorFacturadoProyecto> VValorFacturadoProyecto { get; set; }
        public virtual DbSet<VValorFacturadoSolicitudPago> VValorFacturadoSolicitudPago { get; set; }
        public virtual DbSet<VValorFacturadoXfasesSolicitudPago> VValorFacturadoXfasesSolicitudPago { get; set; }
        public virtual DbSet<VValorTrasladoXproyecto> VValorTrasladoXproyecto { get; set; }
        public virtual DbSet<VValorUsoXcontratoAportante> VValorUsoXcontratoAportante { get; set; }
        public virtual DbSet<VValorUsoXcontratoId> VValorUsoXcontratoId { get; set; }
        public virtual DbSet<VValorUsosFasesAportanteProyecto> VValorUsosFasesAportanteProyecto { get; set; }
        public virtual DbSet<VVerificarSeguimientoSemanal> VVerificarSeguimientoSemanal { get; set; }
        public virtual DbSet<VigenciaAporte> VigenciaAporte { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActuacionSeguimiento>(entity =>
            {
                entity.HasComment("Almacena el seguimiento de las actuaciones relacionadas a las controversias generadas en el sistema");

                entity.Property(e => e.ActuacionSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActuacionAdelantada).HasComment("se debe registrar la actividad que el usuario considere debe realizarse.");

                entity.Property(e => e.CantDiasVencimiento).HasComment("Días de vencimiento de términos para la próxima actuación requerida");

                entity.Property(e => e.ControversiaActuacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsResultadoDefinitivo).HasComment("se debe indicar si el trámite ante la fiduciaria se debe dar por cerrado");

                entity.Property(e => e.EstadoDerivadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoReclamacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacionAdelantada)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de actuación adelantada");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vencimiento del registro");

                entity.Property(e => e.NumeroActuacionReclamacion)
                    .HasMaxLength(100)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProximaActuacion).HasComment("registrar la actividad que considere debe realizarse");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta URL con soportes de la actuación");

                entity.Property(e => e.SeguimientoCodigo).HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.ActuacionSeguimiento)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActuacionSeguimiento_ControversiaActuacion");
            });

            modelBuilder.Entity<AjustePragramacionObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones a los ajustes de la programación");

                entity.Property(e => e.AjustePragramacionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsObra)
                    .HasColumnName("esObra")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.AjusteProgramacion)
                    .WithMany(p => p.AjustePragramacionObservacion)
                    .HasForeignKey(d => d.AjusteProgramacionId)
                    .HasConstraintName("FK_AjustePragramacionObservacion_AjusteProgramacion");
            });

            modelBuilder.Entity<AjusteProgramacion>(entity =>
            {
                entity.HasComment("Almacena ajustes a los tipos de programación");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ArchivoCargueIdFlujoInversion).HasComment("Identificador del archivo de cargue de flujo de inversión");

                entity.Property(e => e.ArchivoCargueIdProgramacionObra).HasComment("Identificador del archivo de cargue de programación de obra");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionDevolucionFlujoInversion).HasComment("Identificador de la observación de la devolución del flujo de inversión");

                entity.Property(e => e.ObservacionDevolucionIdProgramacionObra).HasComment("Identificador de la observación de la devolución de programación de obra");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoValidacion).HasComment("Indica que el registro queda completo de validación");

                entity.Property(e => e.TieneObservacionesFlujoInversion).HasComment("Campo que indica que tiene observaciones de la tabla en mención");

                entity.Property(e => e.TieneObservacionesProgramacionObra).HasComment("Campo que indica que tiene observaciones de la tabla en mención");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.AjusteProgramacion)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .HasConstraintName("FK_AjusteProgramacion_ContratacionProyecto");

                entity.HasOne(d => d.NovedadContractual)
                    .WithMany(p => p.AjusteProgramacion)
                    .HasForeignKey(d => d.NovedadContractualId)
                    .HasConstraintName("FK_AjusteProgramacion_NovedadContractual");
            });

            modelBuilder.Entity<AjusteProgramacionFlujo>(entity =>
            {
                entity.HasComment("Almacena el flujo de los ajustes de programación");

                entity.Property(e => e.AjusteProgramacionFlujoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.MesEjecucionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Semana)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Número de la semana");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.AjusteProgramacion)
                    .WithMany(p => p.AjusteProgramacionFlujo)
                    .HasForeignKey(d => d.AjusteProgramacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AjusteProgramacionFlujo_AjusteProgramacion");
            });

            modelBuilder.Entity<AjusteProgramacionObra>(entity =>
            {
                entity.HasComment("Almacena los ajustes de programación para los proyectos de tipo obra");

                entity.Property(e => e.AjusteProgramacionObraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la actividad");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Duracion).HasComment("Cantidad de semanas que dura la programación");

                entity.Property(e => e.EsRutaCritica).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de la actividad");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasComment("Fecha inicio de la actividad");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.HasOne(d => d.AjusteProgramacion)
                    .WithMany(p => p.AjusteProgramacionObra)
                    .HasForeignKey(d => d.AjusteProgramacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AjusteProgramacionObra_AjusteProgramacion");
            });

            modelBuilder.Entity<AportanteFuenteFinanciacion>(entity =>
            {
                entity.HasComment("Almacena los aportantes relacionados a las fuentes de financiación");

                entity.Property(e => e.AportanteFuenteFinanciacionId).HasComment("Descripción de la siguiente actividad por el área legal");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEdicion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de edición");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProyectoAdministrativoAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioEdicion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que raliza la edición");

                entity.Property(e => e.ValorFuente).HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.AportanteFuenteFinanciacion)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_FuenteFinanciacion_FuenteFinanciacionId");

                entity.HasOne(d => d.ProyectoAdministrativoAportante)
                    .WithMany(p => p.AportanteFuenteFinanciacion)
                    .HasForeignKey(d => d.ProyectoAdministrativoAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoAdministrativoAportante");
            });

            modelBuilder.Entity<ArchivoCargue>(entity =>
            {
                entity.HasComment("Almacena las referencias de los archivos cargados en el sistema");

                entity.Property(e => e.ArchivoCargueId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.CantidadRegistros).HasComment("Cantidad de registros");

                entity.Property(e => e.CantidadRegistrosInvalidos).HasComment("Cantidad de registros invalidos");

                entity.Property(e => e.CantidadRegistrosValidos).HasComment("Cantidad de registros validos");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Nombre del archivo");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.OrigenId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ReferenciaId)
                    .HasColumnName("ReferenciaID")
                    .HasComment("Referencia a la tabla dominio sobre la columna dominio ID");

                entity.Property(e => e.Ruta)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Ruta del archivo de cargue");

                entity.Property(e => e.Tamano)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Indica el tamaño del archivo de cargue");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.HasComment("Almacena las acciones que hace el usuario en el sistema");

                entity.Property(e => e.AuditoriaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AccionId).HasComment("Referencia a la tabla dominio sobre la columna dominio ID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se hace la acción");

                entity.Property(e => e.MensajesValidacionesId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion).HasComment("Complemento de la acción");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que hace la acción");

                entity.HasOne(d => d.Accion)
                    .WithMany(p => p.Auditoria)
                    .HasForeignKey(d => d.AccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_AccionId_Dominio_DominioId");

                entity.HasOne(d => d.MensajesValidaciones)
                    .WithMany(p => p.Auditoria)
                    .HasForeignKey(d => d.MensajesValidacionesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_MensajesValidaciones_MensajesValidacionesId");
            });

            modelBuilder.Entity<BalanceFinanciero>(entity =>
            {
                entity.HasComment("Almacena el balance financiero de los diferentes proyectos existentes en el sistema");

                entity.Property(e => e.BalanceFinancieroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.EstaAnulado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoBalanceCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAnulado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de anulación del BF");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.JustificacionTrasladoAportanteFuente).HasComment("justificacion del traslado de la fuente de un aportante");

                entity.Property(e => e.NumeroBalance)
                    .HasMaxLength(20)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RequiereTransladoRecursos).HasComment("Indica si requiere traslado de recursos");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(1000)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.BalanceFinanciero)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BalanceFinanciero_ContratacionProyecto");
            });

            modelBuilder.Entity<BalanceFinancieroTraslado>(entity =>
            {
                entity.HasComment("Almancena los traslados relacionados a cada uno de los balances financieros");

                entity.Property(e => e.BalanceFinancieroTrasladoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.BalanceFinancieroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAnulacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de anulación del traslado del BF");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroTraslado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorTraslado)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.BalanceFinanciero)
                    .WithMany(p => p.BalanceFinancieroTraslado)
                    .HasForeignKey(d => d.BalanceFinancieroId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BalanceFinanciero_BalanceFinancieroTraslado");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.BalanceFinancieroTraslado)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .HasConstraintName("FK_BalanceFinanciero_OrdenGiro");
            });

            modelBuilder.Entity<BalanceFinancieroTrasladoValor>(entity =>
            {
                entity.HasComment("Almacena el detalle de los traslados relacionados a cada uno de los balances financieros");

                entity.Property(e => e.BalanceFinancieroTrasladoValorId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.BalanceFinancieroTrasladoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsPreconstruccion).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroDetalleDescuentoTecnicaAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.OrdenGiroDetalleDescuentoTecnicaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionDescuentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoTrasladoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorTraslado)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.BalanceFinancieroTraslado)
                    .WithMany(p => p.BalanceFinancieroTrasladoValor)
                    .HasForeignKey(d => d.BalanceFinancieroTrasladoId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValor_BalanceFinancieroTrasladoId");

                entity.HasOne(d => d.OrdenGiroDetalleDescuentoTecnicaAportante)
                    .WithMany(p => p.BalanceFinancieroTrasladoValor)
                    .HasForeignKey(d => d.OrdenGiroDetalleDescuentoTecnicaAportanteId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValor_OrdenGiroDetalleDescuentoTecnicaAportanteId");

                entity.HasOne(d => d.OrdenGiroDetalleDescuentoTecnica)
                    .WithMany(p => p.BalanceFinancieroTrasladoValor)
                    .HasForeignKey(d => d.OrdenGiroDetalleDescuentoTecnicaId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValor_OrdenGiroDetalleDescuentoTecnicaId");

                entity.HasOne(d => d.OrdenGiroDetalleTerceroCausacionAportante)
                    .WithMany(p => p.BalanceFinancieroTrasladoValor)
                    .HasForeignKey(d => d.OrdenGiroDetalleTerceroCausacionAportanteId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValor_OrdenGiroDetalleTerceroCausacionAportante");

                entity.HasOne(d => d.OrdenGiroDetalleTerceroCausacionDescuento)
                    .WithMany(p => p.BalanceFinancieroTrasladoValor)
                    .HasForeignKey(d => d.OrdenGiroDetalleTerceroCausacionDescuentoId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValor_OrdenGiroDetalleTerceroCausacionDescuentoId");
            });

            modelBuilder.Entity<CargueObservacion>(entity =>
            {
                entity.HasComment("Almacena los cargues realizados en el módulo de construcción");

                entity.Property(e => e.CargueObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConstruccionCargueId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Texto de la observación");

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ConstruccionCargue)
                    .WithMany(p => p.CargueObservacion)
                    .HasForeignKey(d => d.ConstruccionCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CargueObservacion_ConstruccionCargue");
            });

            modelBuilder.Entity<CarguePago>(entity =>
            {
                entity.HasComment("Almacena los archivos relacionados a los pagos");

                entity.Property(e => e.CarguePagoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CargueValido).HasComment("Indica que el cargue es valido");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Errores)
                    .IsUnicode(false)
                    .HasComment("Evidencia de error en el cargue");

                entity.Property(e => e.FechaCargue)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se hace el cargue del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.JsonContent)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Contenido del archivo en formato json");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del archivo de cargue");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RegistrosInvalidos).HasComment("Cantidad de registros invalidos");

                entity.Property(e => e.RegistrosValidos).HasComment("Cantidad de registros validos");

                entity.Property(e => e.TotalRegistros).HasComment("Cantidad de registros del archivo");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<CarguePagosRendimientos>(entity =>
            {
                entity.HasKey(e => e.CargaPagosRendimientosId);

                entity.HasComment("Almacena los archivos relacionados a los pagos de rendimientos");

                entity.Property(e => e.CargaPagosRendimientosId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CargueValido).HasComment("Indica que el cargue es valido");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Errores)
                    .IsUnicode(false)
                    .HasComment("Evidencia de error en el cargue");

                entity.Property(e => e.EstadoCargue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Indica el estado del registro");

                entity.Property(e => e.FechaActa)
                    .HasColumnType("date")
                    .HasComment("Fecha del acta");

                entity.Property(e => e.FechaCargue)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se hace el cargue del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.Json)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Contenido del archivo en formato json");

                entity.Property(e => e.MostrarInconsistencias).HasComment("Indica si se deben mostrar las inconsistencias");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del archivo de cargue");

                entity.Property(e => e.NumeroActa).HasComment("Númeo del acta");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.PendienteAprobacion).HasComment("Indica si hay pendiente una aprobación");

                entity.Property(e => e.RegistrosConsistentes).HasComment("Cantidad de registros consistentes");

                entity.Property(e => e.RegistrosInconsistentes).HasComment("Cantidad de registros inconsistentes");

                entity.Property(e => e.RegistrosInvalidos).HasComment("Cantidad de registros invalidos");

                entity.Property(e => e.RegistrosValidos).HasComment("Cantidad de registros validos");

                entity.Property(e => e.RutaActa)
                    .IsUnicode(false)
                    .HasComment("Ruta del acta");

                entity.Property(e => e.TipoCargue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Tipo de cargue");

                entity.Property(e => e.TotalRegistros).HasComment("Cantidad de registros del archivo");

                entity.Property(e => e.TramiteJson).HasComment("Archivo en formato Json");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<Cofinanciacion>(entity =>
            {
                entity.HasComment("Almacena los acuerdos de cofinanciación");

                entity.Property(e => e.CofinanciacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.VigenciaCofinanciacionId).HasComment("Llave foranea a la tabla en mención");
            });

            modelBuilder.Entity<CofinanciacionAportante>(entity =>
            {
                entity.HasComment("Almacena la relación entre el aportante y el acuerdo de cofinanciación");

                entity.HasIndex(e => new { e.CofinanciacionId, e.Eliminado })
                    .HasName("idxconfid_eliminado");

                entity.Property(e => e.CofinanciacionAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CofinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CuentaConRp)
                    .HasColumnName("CuentaConRP")
                    .HasComment("Indica si tiene registro presupuestal");

                entity.Property(e => e.DepartamentoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Referencia a la tabla localización sobre la columna localización ID");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Referencia a la tabla localización sobre la columna localización ID");

                entity.Property(e => e.NombreAportanteId).HasComment("Referencia a la tabla dominio sobre la columna dominio ID");

                entity.Property(e => e.TipoAportanteId).HasComment("Referencia a la tabla dominio sobre la columna dominio ID");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Cofinanciacion)
                    .WithMany(p => p.CofinanciacionAportante)
                    .HasForeignKey(d => d.CofinanciacionId)
                    .HasConstraintName("fk_CofinanciacionAportante_Cofinanciacion_1");

                entity.HasOne(d => d.Departamento)
                    .WithMany(p => p.CofinanciacionAportanteDepartamento)
                    .HasForeignKey(d => d.DepartamentoId)
                    .HasConstraintName("fk_cofinanciacionDepartamento");

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.CofinanciacionAportanteMunicipio)
                    .HasForeignKey(d => d.MunicipioId)
                    .HasConstraintName("fk_cofinanciacionMunicipio");

                entity.HasOne(d => d.NombreAportante)
                    .WithMany(p => p.CofinanciacionAportanteNombreAportante)
                    .HasForeignKey(d => d.NombreAportanteId)
                    .HasConstraintName("fk_cofinanciacionNombre");

                entity.HasOne(d => d.TipoAportante)
                    .WithMany(p => p.CofinanciacionAportanteTipoAportante)
                    .HasForeignKey(d => d.TipoAportanteId)
                    .HasConstraintName("fk_cofinanciacion_tipo");
            });

            modelBuilder.Entity<CofinanciacionDocumento>(entity =>
            {
                entity.HasComment("Almacena los documentos relacionados al aportante de un acuerdo de financiación");

                entity.Property(e => e.CofinanciacionDocumentoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CofinanciacionAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaActa)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del acta");

                entity.Property(e => e.FechaAcuerdo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del acuerdo de cofinanciación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroActa).HasComment("numero de acta");

                entity.Property(e => e.NumeroAcuerdo)
                    .HasColumnType("numeric(25, 0)")
                    .HasComment("Numero de acuerdo");

                entity.Property(e => e.TipoDocumentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorDocumento)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorTotalAportante)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.VigenciaAporte).HasComment("Vigencia del acuerdo de cofinanciación (Dominio)");

                entity.HasOne(d => d.CofinanciacionAportante)
                    .WithMany(p => p.CofinanciacionDocumento)
                    .HasForeignKey(d => d.CofinanciacionAportanteId)
                    .HasConstraintName("Fk_CofinanciacionAportanteId_FK_CofinanciacionAportante_CofinanciacionAportanteId");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.CofinanciacionDocumento)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .HasConstraintName("FK_CofinanciacionTipo_FK_dominio");
            });

            modelBuilder.Entity<ComiteTecnico>(entity =>
            {
                entity.HasComment("Almacena los datos generales de un comité técnico");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantCompromisos).HasComment("Cantidad de compromisos");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAprobado).HasComment("Indica si la solicitud fue aprobado 1. Si, 2. No");

                entity.Property(e => e.EsComiteFiduciario).HasComment("Indica si es un tipo de Comité Fiduciario 0. Comité técnico, 1. Comité Fiduciario  ");

                entity.Property(e => e.EsCompleto).HasComment("Indica si la sesión se completo 1. Completo 0. Incompleto");

                entity.Property(e => e.EstadoActaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoComiteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAplazamiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aplazamiento del comité técnico");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaOrdenDia)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de orden del día del comité técnico");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Si el tema es otro diferente, se presentará un campo de texto abierto de 3,000 caracteres para  que el usuario explique el desarrollo del tema.");

                entity.Property(e => e.NumeroComite)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de comité");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RequiereVotacion).HasComment("Indica que el tema requiere votación");

                entity.Property(e => e.RutaActaSesion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del acta");

                entity.Property(e => e.RutaSoporteVotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Ruta de url de soporte de votación.");

                entity.Property(e => e.TieneCompromisos).HasComment("Indica si la solicitud tiene compromisos 1. SI, 0. No");

                entity.Property(e => e.TipoTemaFiduciarioCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<ComponenteAportante>(entity =>
            {
                entity.HasComment("Almacena el dinero de cada aportante sobre cada fase");

                entity.Property(e => e.ComponenteAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el Componente esta activo dentro del proyecto (0.Inactivo 1. Activo)");

                entity.Property(e => e.ContratacionProyectoAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FaseCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro esta completo");

                entity.Property(e => e.TipoComponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratacionProyectoAportante)
                    .WithMany(p => p.ComponenteAportante)
                    .HasForeignKey(d => d.ContratacionProyectoAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteAportante_ContratacionProyectoAportante");
            });

            modelBuilder.Entity<ComponenteAportanteNovedad>(entity =>
            {
                entity.HasComment("Almacena las novedades sobre cada componente");

                entity.Property(e => e.ComponenteAportanteNovedadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.CofinanciacionAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FaseCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NovedadContractualAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica si el Componente esta activo dentro del proyecto (0.Inactivo 1. Activo)");

                entity.Property(e => e.TipoComponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.CofinanciacionAportante)
                    .WithMany(p => p.ComponenteAportanteNovedad)
                    .HasForeignKey(d => d.CofinanciacionAportanteId)
                    .HasConstraintName("FK_ComponenteAportanteNovedad_CofinanciacionAportante");

                entity.HasOne(d => d.NovedadContractualAportante)
                    .WithMany(p => p.ComponenteAportanteNovedad)
                    .HasForeignKey(d => d.NovedadContractualAportanteId)
                    .HasConstraintName("FK_ComponenteAportanteNovedad_ComponenteAportanteNovedad");
            });

            modelBuilder.Entity<ComponenteFuenteNovedad>(entity =>
            {
                entity.HasComment("Almacena las fuentes de cada aportante para cada fase del proyecto");

                entity.Property(e => e.ComponenteFuenteNovedadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComponenteAportanteNovedadId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComponenteAportanteNovedad)
                    .WithMany(p => p.ComponenteFuenteNovedad)
                    .HasForeignKey(d => d.ComponenteAportanteNovedadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteFuenteNovedad_ComponenteAportanteNovedad");
            });

            modelBuilder.Entity<ComponenteUso>(entity =>
            {
                entity.HasComment("Almacena los usos de cada fuente para cada fase del proyecto");

                entity.Property(e => e.ComponenteUsoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el Componente esta activo dentro del proyecto (0.Inactivo 1. Activo)");

                entity.Property(e => e.ComponenteAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorUso)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ComponenteAportante)
                    .WithMany(p => p.ComponenteUso)
                    .HasForeignKey(d => d.ComponenteAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteUso_ComponenteAportante");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.ComponenteUso)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_ComponenteUso_FuenteFinanciacion");
            });

            modelBuilder.Entity<ComponenteUsoNovedad>(entity =>
            {
                entity.HasComment("Almacena las novedades relacionadas a los usos para cada fase del proyecto");

                entity.Property(e => e.ComponenteUsoNovedadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.ComponenteFuenteNovedadId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorUso)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ComponenteFuenteNovedad)
                    .WithMany(p => p.ComponenteUsoNovedad)
                    .HasForeignKey(d => d.ComponenteFuenteNovedadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteUsoNovedad_ComponenteFuenteNovedad");
            });

            modelBuilder.Entity<CompromisoSeguimiento>(entity =>
            {
                entity.HasComment("Almacena los compromisos de una sesión de comité técnico");

                entity.Property(e => e.CompromisoSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.DescripcionSeguimiento)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Descripción del seguimiento");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoCompromisoCodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.SesionComiteTecnicoCompromisoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionSolicitudCompromisoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TemaCompromisoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SesionComiteTecnicoCompromiso)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.SesionComiteTecnicoCompromisoId)
                    .HasConstraintName("FK_CompromisoSeguimiento_SesionComiteTecnicoCompromiso");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .HasConstraintName("FK_CompromisoSeguimiento_SesionParticipante");

                entity.HasOne(d => d.SesionSolicitudCompromiso)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.SesionSolicitudCompromisoId)
                    .HasConstraintName("FK_CompromisoSeguimiento_SesionSolicitudCompromiso");

                entity.HasOne(d => d.TemaCompromiso)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.TemaCompromisoId)
                    .HasConstraintName("FK_CompromisoSeguimiento_TemaCompromiso");
            });

            modelBuilder.Entity<ConceptoPagoUso>(entity =>
            {
                entity.HasComment("Almacena los códigos del concepto del pago");

                entity.Property(e => e.ConceptoPagoUsoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Uso)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Identificador del uso");
            });

            modelBuilder.Entity<ConstruccionCargue>(entity =>
            {
                entity.HasComment("Almacena los registros de los cargues hechos en el módulo de construcción");

                entity.Property(e => e.ConstruccionCargueId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantRegistrosInvalidos).HasComment("Cantidad de registros invalidos del programa");

                entity.Property(e => e.CantRegistrosValidos).HasComment("Cantidad de registros validos del programa");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EstadoCargueCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCargue)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se hace el cargue del registro");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.TipoCargueCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TotalRegistros).HasComment("Cantidad total de registros");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionCargue)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionCargue_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones del módulo de construcción");

                entity.Property(e => e.ConstruccionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsActa).HasComment("indica que la observación es del Acta.");

                entity.Property(e => e.EsSupervision).HasComment("indica que la observación es de la Supervision");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.TipoObservacionConstruccion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Dominio, Tipos de Observaciones realizadas en toda la fase de Construcción. (1.Diagnostico, 2. Planes y programas  3. Plan Licencia Vigente, 4. Plan Cambio Constructor, 5. Plan Acta Aceptación y Apropiación, 6. Plan de residuos de construcción y  demolición (RCD), etc)");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionObservacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .HasConstraintName("FK_ConstruccionObservacion_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionPerfil>(entity =>
            {
                entity.HasComment("Almacena los perfiles requeridos para un proyecto de la fase de construcción");

                entity.Property(e => e.ConstruccionPerfilId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadHvAprobadas).HasComment("Cantidad de hojas de vida aprobadas para cada perfil ");

                entity.Property(e => e.CantidadHvRecibidas).HasComment("Cantidad de hojas de vida recibidas para cada perfil ");

                entity.Property(e => e.CantidadHvRequeridas).HasComment("Cantidad de  hojas de vida  requeridas para  cada perfil");

                entity.Property(e => e.ConObervacionesSupervision).HasComment("Indica que se tienen observaciones al perfil por parte del supervisor");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroRadicadoFfie)
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de aprobación de Hojas de vida");

                entity.Property(e => e.NumeroRadicadoFfie1)
                    .HasColumnName("NumeroRadicadoFFIE1")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de aprobación de Hojas de vida");

                entity.Property(e => e.NumeroRadicadoFfie2)
                    .HasColumnName("NumeroRadicadoFFIE2")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de aprobación de Hojas de vida");

                entity.Property(e => e.NumeroRadicadoFfie3)
                    .HasColumnName("NumeroRadicadoFFIE3")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de aprobación de Hojas de vida");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.PerfilCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RutaSoporte).HasComment("Ruta del perfil de construcción");

                entity.Property(e => e.TieneObservacionesApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionPerfil)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionPerfil_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionPerfilNumeroRadicado>(entity =>
            {
                entity.HasComment("Almacena el número de radicado relacionado al perfil almacenado para un proyecto de la fase de construcción");

                entity.Property(e => e.ConstruccionPerfilNumeroRadicadoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConstruccionPerfilId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroRadicado)
                    .HasMaxLength(50)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ConstruccionPerfil)
                    .WithMany(p => p.ConstruccionPerfilNumeroRadicado)
                    .HasForeignKey(d => d.ConstruccionPerfilId)
                    .HasConstraintName("FK_ConstruccionPerfilNumeroRadicado_ConstruccionPerfil");
            });

            modelBuilder.Entity<ConstruccionPerfilObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones realcionadas al perfil de un proyecto en la fase de construcción");

                entity.Property(e => e.ConstruccionPerfilObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.ConstruccionPerfilId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ConstruccionPerfil)
                    .WithMany(p => p.ConstruccionPerfilObservacion)
                    .HasForeignKey(d => d.ConstruccionPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionPerfilObservacion_ConstruccionPerfil");
            });

            modelBuilder.Entity<Contratacion>(entity =>
            {
                entity.HasComment("Almacena las diferentes contrataciones de todos los contratistas");

                entity.Property(e => e.ContratacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConsideracionDescripcion).HasComment("Campo abierto para que el usuario describa la consideración especial");

                entity.Property(e => e.ContratistaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsMultiProyecto).HasComment("0. No, 1. Si");

                entity.Property(e => e.EsObligacionEspecial).HasComment("¿Este contrato tendrá alguna obligación especial? 0.No 1. Si");

                entity.Property(e => e.Estado).HasComment("Indica el estado del registro");

                entity.Property(e => e.EstadoAprobacionLiquidacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoTramiteLiquidacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado del tramite de liquidación");

                entity.Property(e => e.EstadoValidacionLiquidacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaAprobacionLiquidacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de liquidación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEnvioDocumentacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio de la documentación de la contratación");

                entity.Property(e => e.FechaFirmaContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la firma del contratista en la contratación");

                entity.Property(e => e.FechaFirmaEnvioContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la firma para envio a contratista");

                entity.Property(e => e.FechaFirmaEnvioFiduciaria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la firma para envio a fiduciaria");

                entity.Property(e => e.FechaFirmaFiduciaria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la firma de la fiduciaria en la contratación");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.FechaTramiteGestionar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.FechaTramiteLiquidacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.FechaTramiteLiquidacionControl)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.FechaValidacionLiquidacion)
                    .HasColumnType("date")
                    .HasComment("Fecha de valiación de la liquidación");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("número automático de solicitud a comité precedido de  las siglas PI, para los proyectos de infraestructura.");

                entity.Property(e => e.NumeroSolicitudLiquidacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ObservacionGestionar).HasComment("Observaciones al gestionar");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionesLiquidacion).HasComment("Observaciones de liquidación de contratación");

                entity.Property(e => e.PlazoContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompleto1).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoAprobacionLiquidacion).HasComment("Indica que el registro queda completo aprobación de liquidación");

                entity.Property(e => e.RegistroCompletoGestionar).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoLiquidacion)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro queda completo por verificaión de liquidación");

                entity.Property(e => e.RegistroCompletoTramiteLiquidacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificacionLiquidacion).HasComment("Indica que el registro queda completo por aprobación de liquidación");

                entity.Property(e => e.RutaMinuta)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("ruta de la minuta ");

                entity.Property(e => e.TipoContratacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlDocumentoLiquidacion)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UrlSoporteGestionar).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contratista)
                    .WithMany(p => p.Contratacion)
                    .HasForeignKey(d => d.ContratistaId)
                    .HasConstraintName("FK_Contratacion_Contratista");

                entity.HasOne(d => d.PlazoContratacion)
                    .WithMany(p => p.Contratacion)
                    .HasForeignKey(d => d.PlazoContratacionId)
                    .HasConstraintName("FK__Contratac__Plazo__39788055");
            });

            modelBuilder.Entity<ContratacionObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones de la contratación");

                entity.Property(e => e.ContratacionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.ContratacionObservacion)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .HasConstraintName("Fk_ComiteTecnicoId_ComiteTecnico");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.ContratacionObservacion)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ContratacionId_Contratacion");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.ContratacionObservacion)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ContratacionProyectId_ContratacionProyecto");
            });

            modelBuilder.Entity<ContratacionProyecto>(entity =>
            {
                entity.HasComment("Almacena las contrataciones de los proyecto");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.AvanceFisicoSemanal)
                    .HasColumnType("decimal(18, 3)")
                    .HasComment("Valorsegún unidad de medida");

                entity.Property(e => e.ContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAvanceobra).HasComment("¿El proyecto tiene avance de obra?  si/no.");

                entity.Property(e => e.EsReasignacion).HasComment("¿El proyecto es una reasignación?,  opciones Si/No.");

                entity.Property(e => e.EstadoObraCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoRequisitosVerificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacionRequisitos)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de requisitos");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVigencia)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vigencia");

                entity.Property(e => e.LicenciaVigente).HasComment("¿El proyecto tiene licencia vigente? 0. No 1. Si");

                entity.Property(e => e.NumeroLicencia)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de licencia");

                entity.Property(e => e.PorcentajeAvanceObra)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("¿Cuál es el porcentaje de avance de obra?");

                entity.Property(e => e.ProgramacionSemanal)
                    .HasColumnType("decimal(18, 3)")
                    .HasComment("Cuantia de la programación semanal");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RequiereLicencia).HasComment("¿El proyecto requiere licencias? 0 No 1 Si");

                entity.Property(e => e.RutaCargaActaTerminacionContrato)
                    .HasMaxLength(200)
                    .HasComment("Ruta de la carga del acta");

                entity.Property(e => e.TieneMonitoreoWeb).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.ContratacionProyecto)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratacionProyecto_Contratacion");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ContratacionProyecto)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratacionProyecto_Proyecto");
            });

            modelBuilder.Entity<ContratacionProyectoAportante>(entity =>
            {
                entity.HasComment("Almacena las relaciones de los aportantes con las contrataciones y proyectos");

                entity.Property(e => e.ContratacionProyectoAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CofinanciacionAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAporte)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.CofinanciacionAportante)
                    .WithMany(p => p.ContratacionProyectoAportante)
                    .HasForeignKey(d => d.CofinanciacionAportanteId)
                    .HasConstraintName("FK_Cofinancaicion");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.ContratacionProyectoAportante)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratacionProyectoAportante_ContratacionProyecto");
            });

            modelBuilder.Entity<Contratista>(entity =>
            {
                entity.HasComment("Almacena los contratistas");

                entity.Property(e => e.ContratistaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre de la Entidad");

                entity.Property(e => e.NumeroIdentificacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de identificación");

                entity.Property(e => e.NumeroInvitacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de la invitación");

                entity.Property(e => e.ProcesoSeleccionProponenteId)
                    .HasColumnName("ProcesoSeleccionProponenteID")
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RepresentanteLegal)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del Representante Legal");

                entity.Property(e => e.RepresentanteLegalNumeroIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de identificación del representante legal del contratista de obra  ");

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoProponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccionProponente)
                    .WithMany(p => p.Contratista)
                    .HasForeignKey(d => d.ProcesoSeleccionProponenteId)
                    .HasConstraintName("FK_Contratista_ProcesoSeleccionProponente");
            });

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.HasComment("Almacena los contratos");

                entity.Property(e => e.ContratoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ApoyoId).HasComment("Llave foranea a la tabla usuario");

                entity.Property(e => e.CantidadPerfiles).HasComment("Cantidad de perfiles del contrato");

                entity.Property(e => e.ConObervacionesActa).HasComment("Acta con obseraciones");

                entity.Property(e => e.ConObervacionesActaFase1).HasComment("Indica que tiene observaciones del acta fase 1");

                entity.Property(e => e.ConObervacionesActaFase2).HasComment("Indica que tiene observaciones del acta fase 2");

                entity.Property(e => e.ContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstaDevuelto).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.Estado).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EstadoActa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Estado del acta");

                entity.Property(e => e.EstadoActaFase2)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasComment("Estado del Acta Precontractual Dominio (1. Sin Acta generada, 2. “Con acta en proceso de firma”,3. “Con acta suscrita y cargada” )");

                entity.Property(e => e.EstadoDocumentoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoFase1Diagnostico).HasComment("Indica el estado del diagnostico de la fase 1");

                entity.Property(e => e.EstadoFase1EyD).HasComment("Indica el estado de la fase 1");

                entity.Property(e => e.EstadoFase2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Dominio Estado de Fase1 interventoria (1.Estudios y diseño, )");

                entity.Property(e => e.EstadoVerificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoVerificacionConstruccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActaInicioFase1)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del acta de inicio fase 1");

                entity.Property(e => e.FechaActaInicioFase2)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del acta de inicio fase 2");

                entity.Property(e => e.FechaAprobacionRequisitos)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosApoyo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de apoyo de la supervisión para requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionApoyo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de apoyo de la supervisión de la construcción para requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionInterventor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de interventor de la construcción para requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de la supervisión de la consturcción para requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosInterventor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación del interventor para requisitos");

                entity.Property(e => e.FechaAprobacionRequisitosSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de supervisor para requisitos");

                entity.Property(e => e.FechaCambioEstadoFase2)
                    .HasColumnType("datetime")
                    .HasComment("Fecha cambio estado fase 2 del contrato");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEnvioFirma)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio de firma del contrato");

                entity.Property(e => e.FechaFirmaActaContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de l acta del contratista del contrato");

                entity.Property(e => e.FechaFirmaActaContratistaFase1)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de fase 1");

                entity.Property(e => e.FechaFirmaActaContratistaFase2)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de fase 2");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta por el interventor");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoriaFase1)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de fase 1 por interventor");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoriaFase2)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de fase 2 por interventor");

                entity.Property(e => e.FechaFirmaContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de firma del contratista del contrato");

                entity.Property(e => e.FechaFirmaContrato)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del contrato");

                entity.Property(e => e.FechaFirmaFiduciaria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de la fiduciaria del contrato");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaTerminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de terminación del contrato");

                entity.Property(e => e.FechaTerminacionFase2)
                    .HasColumnType("datetime")
                    .HasComment("Fecha terminación de la fase 2 del contrato");

                entity.Property(e => e.FechaTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.InterventorId).HasComment("Llave foranea a la tabla usuario");

                entity.Property(e => e.ModalidadCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de solicitud");

                entity.Property(e => e.ObservacionConsideracionesEspeciales).HasComment("Observaciones de las consideraciones especiales");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.PlazoFase1PreDias).HasComment("Plazo de ejecución fase 1 – Preconstrucción: Días  ");

                entity.Property(e => e.PlazoFase1PreMeses).HasComment("Plazo de ejecución fase 1 – Preconstrucción: Meses:");

                entity.Property(e => e.PlazoFase2ConstruccionDias).HasComment("Plazo de ejecución fase Construcción: Días  ");

                entity.Property(e => e.PlazoFase2ConstruccionMeses).HasComment("Plazo de ejecución fase 2 Construcción: Meses:");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompleto1).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoConstruccion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RutaActa)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Ruta del acta");

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Ruta del Acta Fase 1 PreConstrucción");

                entity.Property(e => e.RutaActaFase2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Ruta del Acta Fase Construcción");

                entity.Property(e => e.RutaActaSuscrita)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Ruta del Acta suscrita Fase Construcción");

                entity.Property(e => e.RutaDocumento)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del documento del contrato");

                entity.Property(e => e.SupervisorId).HasComment("Llave foranea a la tabla usuario");

                entity.Property(e => e.TieneDiagnosticoFase1).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneEstudiosDisenosFase1).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoContratoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Apoyo)
                    .WithMany(p => p.ContratoApoyo)
                    .HasForeignKey(d => d.ApoyoId)
                    .HasConstraintName("FK_Contrato_Apoyo");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contrato_Contratacion");

                entity.HasOne(d => d.Interventor)
                    .WithMany(p => p.ContratoInterventor)
                    .HasForeignKey(d => d.InterventorId)
                    .HasConstraintName("FK_Contrato_Interventor");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.ContratoSupervisor)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_Contrato_Supervisor");
            });

            modelBuilder.Entity<ContratoConstruccion>(entity =>
            {
                entity.HasComment("Almacena los contratos relacionados a la fase de construcción");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActaApropiacionConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) Acta aceptación y apropiación diseños");

                entity.Property(e => e.ActaApropiacionFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de acta de apropiación");

                entity.Property(e => e.ActaApropiacionFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del acta de apropiación");

                entity.Property(e => e.ActaApropiacionObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones al acta de apropiación");

                entity.Property(e => e.Administracion)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Valor de la cuota de administración");

                entity.Property(e => e.AprovechamientoForestalApropiacionFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado de aprovechamiento forestal");

                entity.Property(e => e.AprovechamientoForestalConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan aprovechamiento forestal aprobado");

                entity.Property(e => e.AprovechamientoForestalFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación aprovechamiento forestal");

                entity.Property(e => e.AprovechamientoForestalObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del aprovechamiento forestal");

                entity.Property(e => e.ArchivoCargueIdFlujoInversion).HasComment("Identificador del archivo de cargue de flujo de inversión");

                entity.Property(e => e.ArchivoCargueIdProgramacionObra).HasComment("Identificador del archivo de cargue de programación de obra");

                entity.Property(e => e.AseguramientoCalidadConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de aseguramiento de la calidad de obra aprobado");

                entity.Property(e => e.AseguramientoCalidadFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de Aprobación deplan de aseguramiento de la calidad de  obra aprobado");

                entity.Property(e => e.AseguramientoCalidadFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado de aseguramiento de calidad");

                entity.Property(e => e.AseguramientoCalidadObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del aseguramiento de calidad");

                entity.Property(e => e.CambioConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) Cambio Constructor de la licencia");

                entity.Property(e => e.CambioFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de cambio");

                entity.Property(e => e.CambioFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha radicado del cambio");

                entity.Property(e => e.CambioObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones al haber un cambio");

                entity.Property(e => e.CantidadHojasVidaContratistaObra).HasComment("Cantidad de Hojas de Vida del contratista de Obra");

                entity.Property(e => e.CantidadPerfilesInterventoria).HasComment("Cuántos perfiles diferentes se requieren del interventor para ejecutar el proyecto");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CostoDirecto)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Costo directo");

                entity.Property(e => e.EsInformeDiagnostico).HasComment("¿Cuenta con informe de diagnóstico aprobado por la interventoría? (1.Si, 0. No)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaInicioObra)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de inicio de obra");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Imprevistos)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("I (Imprevistos)");

                entity.Property(e => e.InventarioArboreoConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan inventario arbóreo (talas) aprobado");

                entity.Property(e => e.InventarioArboreoFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de inventario arboreo");

                entity.Property(e => e.InventarioArboreoFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado  del inventario arboreo");

                entity.Property(e => e.InventarioArboreoObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del inventario arboreo");

                entity.Property(e => e.LicenciaConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No)");

                entity.Property(e => e.LicenciaFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de Aprobación de licencia vigente");

                entity.Property(e => e.LicenciaFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado Licencia Vigente");

                entity.Property(e => e.LicenciaObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de la licencia");

                entity.Property(e => e.ManejoAguasLluviasConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de manejo de aguas lluvias aprobado");

                entity.Property(e => e.ManejoAguasLluviasFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación manejo de aguas de lluvias");

                entity.Property(e => e.ManejoAguasLluviasFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado de manejo de aguas lluvias");

                entity.Property(e => e.ManejoAguasLluviasObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del manejo de aguas lluvias");

                entity.Property(e => e.ManejoAmbientalConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de manejo ambiental aprobado");

                entity.Property(e => e.ManejoAmbientalFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de manejo ambiental");

                entity.Property(e => e.ManejoAmbientalFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del manejo ambiental");

                entity.Property(e => e.ManejoAmbientalObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del manejo ambiental");

                entity.Property(e => e.ManejoAnticipoConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) Manejo del Anticipo");

                entity.Property(e => e.ManejoAnticipoCronogramaAmortizacion).HasComment("¿Cuenta con cronograma de amortización aprobado? Sí No");

                entity.Property(e => e.ManejoAnticipoPlanInversion).HasComment("¿Cuenta con plan de inversión aprobado para el anticipo? Sí No");

                entity.Property(e => e.ManejoAnticipoRequiere).HasComment("¿El contrato requiere anticipo? 1.Si 0. No  ");

                entity.Property(e => e.ManejoAnticipoRutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del soporte del Manejo de anticipo de la fase de construcción");

                entity.Property(e => e.ManejoTransitoConObservaciones1).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de manejo de tránsito (PMT) aprobado");

                entity.Property(e => e.ManejoTransitoFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de manejo de transito");

                entity.Property(e => e.ManejoTransitoFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado  manejo de transito");

                entity.Property(e => e.ManejoTransitoObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del manejo de transito");

                entity.Property(e => e.NumeroSolicitudModificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Numero de solicitud");

                entity.Property(e => e.ObservacionDiagnosticoSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionFlujoInversionSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionManejoAnticipoSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionPlanesProgramasSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionProgramacionObraSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.PlanActaApropiacion).HasComment("Indica que se recibio requisito Acta aceptación y apropiación diseños");

                entity.Property(e => e.PlanAprovechamientoForestal).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanAseguramientoCalidad).HasComment("Indica que se recibio requisito plan de raseguramiento de la calidad de  obra aprobado");

                entity.Property(e => e.PlanCambioConstructorLicencia).HasComment("Cambio constructor responsable de la licencia");

                entity.Property(e => e.PlanInventarioArboreo).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanLicenciaVigente).HasComment("Licencia vigente");

                entity.Property(e => e.PlanManejoAguasLluvias).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanManejoAmbiental).HasComment("Indica que se recibio requisito plan de manejo ambiental aprobado");

                entity.Property(e => e.PlanManejoTransito).HasComment("Indica que se recibio requisito plan de manejo de tránsito (PMT)  aprobado");

                entity.Property(e => e.PlanProgramaSalud).HasComment("Indica que se recibio requisito plan programa de Salud  aprobado");

                entity.Property(e => e.PlanProgramaSeguridad).HasComment("Indica que se recibio requisito plan programa de Seguridad industrial  aprobado");

                entity.Property(e => e.PlanResiduosDemolicion).HasComment("Indica que se recibio requisito plan de residuos de construcción y  demolición (RCD) aprobado");

                entity.Property(e => e.PlanRutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del soporte del plan de la fase de construcción");

                entity.Property(e => e.ProgramaSaludConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) planplan programa de Salud  aprobado");

                entity.Property(e => e.ProgramaSaludFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de programa de salud");

                entity.Property(e => e.ProgramaSaludFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del programa de salud");

                entity.Property(e => e.ProgramaSaludObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del programa de la salud");

                entity.Property(e => e.ProgramaSeguridadConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de programa de Seguridad industrial  aprobado");

                entity.Property(e => e.ProgramaSeguridadFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de aseguramiento de calidad");

                entity.Property(e => e.ProgramaSeguridadFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del programa de seguridad");

                entity.Property(e => e.ProgramaSeguridadObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones del programa de seguridad");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoDiagnostico).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoFlujoInversion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoManejoAnticipo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoPlanesProgramas).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoProgramacionObra).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoValidacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereModificacionContractual).HasComment("¿Se requirió modificación contractual?");

                entity.Property(e => e.ResiduosDemolicionConObservaciones).HasComment("Indica que tiene Observaciones (1.Si, 0.No) plan de residuos de construcción y demolición (RCD) aprobado");

                entity.Property(e => e.ResiduosDemolicionFechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación de residuos de demolición");

                entity.Property(e => e.ResiduosDemolicionFechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del manejo deresiduos de demolición");

                entity.Property(e => e.ResiduosDemolicionObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de residuos de demoliciones");

                entity.Property(e => e.RutaInforme)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del informe de diagnostico con soporte");

                entity.Property(e => e.TieneObservacionesDiagnosticoApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesDiagnosticoSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.TieneObservacionesFlujoInversionApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesFlujoInversionSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.TieneObservacionesManejoAnticipoApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesManejoAnticipoSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.TieneObservacionesPlanesProgramasApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesPlanesProgramasSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.TieneObservacionesProgramacionObraApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesProgramacionObraSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.Utilidad)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("U (Utilidad)");

                entity.Property(e => e.ValorTotalFaseConstruccion)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoConstruccion)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoConstruccion_Contrato");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ContratoConstruccion)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoConstruccion_Proyecto");
            });

            modelBuilder.Entity<ContratoObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones del contrato");

                entity.Property(e => e.ContratoObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EsActa).HasComment("Indica si es acta");

                entity.Property(e => e.EsActaFase1).HasComment("indica que la observación es del Acta para la fase 1 Preconstruccion.");

                entity.Property(e => e.EsActaFase2).HasComment("indica que la observación es del Acta. de la fase 2");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoObservacion)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoObservacion_Contrato");
            });

            modelBuilder.Entity<ContratoPerfil>(entity =>
            {
                entity.HasComment("Almacena la relación de los perfiles al contrato");

                entity.Property(e => e.ContratoPerfilId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadHvAprobadas).HasComment("Cantidad de hojas de vida aprobadas para cada perfil ");

                entity.Property(e => e.CantidadHvRecibidas).HasComment("Cantidad de hojas de vida recibidas para cada perfil ");

                entity.Property(e => e.CantidadHvRequeridas).HasComment("Cantidad de  hojas de vida  requeridas para  cada perfil");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.PerfilCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoPerfilesProyecto)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(400)
                    .HasComment("Ruta donde se encuentra ubicado");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoPerfil)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPerfil_Contrato");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ContratoPerfil)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPerfil_Proyecto");
            });

            modelBuilder.Entity<ContratoPerfilNumeroRadicado>(entity =>
            {
                entity.HasComment("Almacena el número de radicadode los perfiles relacionados a un contrato");

                entity.Property(e => e.ContratoPerfilNumeroRadicadoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPerfilId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroRadicado)
                    .HasMaxLength(50)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPerfil)
                    .WithMany(p => p.ContratoPerfilNumeroRadicado)
                    .HasForeignKey(d => d.ContratoPerfilId)
                    .HasConstraintName("FK_ContratoPerfilNumeroRadicado_ContratoPerfil");
            });

            modelBuilder.Entity<ContratoPerfilObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones de los perfiles relacionados a los contratos");

                entity.Property(e => e.ContratoPerfilObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPerfilId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(3250)
                    .HasComment("Observaciones del contrato");

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPerfil)
                    .WithMany(p => p.ContratoPerfilObservacion)
                    .HasForeignKey(d => d.ContratoPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPerfilObservacion_ContratoPerfil");
            });

            modelBuilder.Entity<ContratoPoliza>(entity =>
            {
                entity.HasComment("Almacena las pólizas relacionadas a los contratos");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DescripcionModificacion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Descripción de la modificación");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoPolizaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaExpedicion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de expedición de la póliza");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.IncluyeCondicionesGenerales).HasComment("¿Se incluyen las  condiciones  generales de la  póliza/ o su  clausulado?");

                entity.Property(e => e.NombreAseguradora)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre de aseguradora");

                entity.Property(e => e.NumeroCertificado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número de Certificado");

                entity.Property(e => e.NumeroPoliza)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("número de la póliza.");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoPoliza)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPoliza_Contrato");
            });

            modelBuilder.Entity<ContratoPolizaActualizacion>(entity =>
            {
                entity.HasComment("Almacena la actualización de las pólizas relacionados a los contratos");

                entity.Property(e => e.ContratoPolizaActualizacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActualizacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado del registro");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaExpedicionActualizacionPoliza)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de expedición de la actualización poliza");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroActualizacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ObservacionEspecifica)
                    .HasMaxLength(2500)
                    .HasComment("Obaservaciones especificas");

                entity.Property(e => e.RazonActualizacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionEspecifica).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionEspecifica).HasComment("Campo que indica que tiene observaciones especificas");

                entity.Property(e => e.TipoActualizacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.ContratoPolizaActualizacion)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .HasConstraintName("FK_ContratoPolizaActualizacion_ContratoPoliza");
            });

            modelBuilder.Entity<ContratoPolizaActualizacionListaChequeo>(entity =>
            {
                entity.HasComment("Almacena la lista de chequeo sobre la actualización de una póliza relacionada a un contrato");

                entity.Property(e => e.ContratoPolizaActualizacionListaChequeoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaActualizacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CumpleDatosAseguradoBeneficiario).HasComment("Indica si cumple datos del beneficiario asegurado");

                entity.Property(e => e.CumpleDatosBeneficiarioGarantiaBancaria).HasComment("Indica si cumple datos del beneficiario de garantia bancaria");

                entity.Property(e => e.CumpleDatosTomadorAfianzado).HasComment("Indica si cumple datos del tomador afianzado");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TieneCondicionesGeneralesPoliza).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneReciboPagoDatosRequeridos).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPolizaActualizacion)
                    .WithMany(p => p.ContratoPolizaActualizacionListaChequeo)
                    .HasForeignKey(d => d.ContratoPolizaActualizacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPolizaActualizacionListaChequeo_ContratoPolizaActualizacion");
            });

            modelBuilder.Entity<ContratoPolizaActualizacionRevisionAprobacionObservacion>(entity =>
            {
                entity.HasComment("Almacena la revisión y aprobación con sus observaciones sobre la actualización de una póliza relacionada a un contrato");

                entity.Property(e => e.ContratoPolizaActualizacionRevisionAprobacionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.ContratoPolizaActualizacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoSegundaRevision)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado del registro");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionGeneral)
                    .HasMaxLength(4000)
                    .HasComment("Observaciones generales");

                entity.Property(e => e.RegistroCompleto)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro queda completo");

                entity.Property(e => e.ResponsableAprobacionId).HasComment("Llave foranea a la tabla usuario");

                entity.Property(e => e.SegundaFechaRevision)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de segunda revisión");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPolizaActualizacion)
                    .WithMany(p => p.ContratoPolizaActualizacionRevisionAprobacionObservacion)
                    .HasForeignKey(d => d.ContratoPolizaActualizacionId)
                    .HasConstraintName("FK_ContratoPolizaActualizacionObservacion_ContratoPolizaActualizacion");

                entity.HasOne(d => d.ResponsableAprobacion)
                    .WithMany(p => p.ContratoPolizaActualizacionRevisionAprobacionObservacion)
                    .HasForeignKey(d => d.ResponsableAprobacionId)
                    .HasConstraintName("FK_ContratoPolizaActualizacionObservacion_ResponsableAprobacion");
            });

            modelBuilder.Entity<ContratoPolizaActualizacionSeguro>(entity =>
            {
                entity.HasComment("Almacena el seguro relacionado a la actualización de una póliza");

                entity.Property(e => e.ContratoPolizaActualizacionSeguroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaActualizacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSeguro)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de seguro");

                entity.Property(e => e.FechaVigenciaAmparo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vigencia del amparo");

                entity.Property(e => e.RegistroCompletoActualizacion)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoSeguro)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneFechaSeguro).HasComment("Indica que tiene ficha de seguro");

                entity.Property(e => e.TieneFechaVigenciaAmparo).HasComment("Indica que tiene fecha de vigencia amparo");

                entity.Property(e => e.TieneValorAmparo).HasComment("Indica que la póliza tiene un valor de amparo");

                entity.Property(e => e.TipoSeguroCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAmparo)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ContratoPolizaActualizacion)
                    .WithMany(p => p.ContratoPolizaActualizacionSeguro)
                    .HasForeignKey(d => d.ContratoPolizaActualizacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPolizaActualizacionSeguro_ContratoPolizaActulizacion");
            });

            modelBuilder.Entity<ControlRecurso>(entity =>
            {
                entity.HasComment("Almacena la relación entre la fuente de financiación, las cuentas bancarias y el registro presupuestal");

                entity.Property(e => e.ControlRecursoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CuentaBancariaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaConsignacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de consignación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorConsignacion)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.VigenciaAporteId).HasComment("Llave foranea a la tabla en mención");

                entity.HasOne(d => d.CuentaBancaria)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.CuentaBancariaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControlRecurso_CuentaBancaria");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControlRecurso_FuenteFinanciacion");

                entity.HasOne(d => d.RegistroPresupuestal)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.RegistroPresupuestalId)
                    .HasConstraintName("FK_ControlRecurso_RegistroPresupuestal");
            });

            modelBuilder.Entity<ControversiaActuacion>(entity =>
            {
                entity.HasComment("Almacena la actuación de una controversia contractual");

                entity.Property(e => e.ControversiaActuacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActuacionAdelantadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ActuacionAdelantadaOtro).HasComment("registrar la actividad que considere debe realizarse.");

                entity.Property(e => e.CantDiasVencimiento).HasComment("registrar la actividad que considere debe realizarse");

                entity.Property(e => e.ControversiaContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EsCompletoReclamacion).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsRequiereAseguradora).HasComment("¿El trámite requiere reclamación ante la aseguradora?");

                entity.Property(e => e.EsRequiereComite).HasComment("¿Está actuación requiere comité técnico?");

                entity.Property(e => e.EsRequiereComiteReclamacion).HasComment("¿Se requiere presentar la propuesta de reclamación a comité técnico?");

                entity.Property(e => e.EsRequiereContratista).HasComment("¿Está actuación requiere la participación o insumo del contratista?");

                entity.Property(e => e.EsRequiereFiduciaria).HasComment("¿Está actuación requiere la participación o insumo de la fiduciaria?");

                entity.Property(e => e.EsRequiereInterventor).HasComment("¿Está actuación requiere la participación o insumo del interventor del contrato?");

                entity.Property(e => e.EsRequiereJuridico).HasComment("¿Está actuación requiere la participación o insumo del equipo jurídico del FFIE?");

                entity.Property(e => e.EsRequiereMesaTrabajo).HasComment("¿El proceso requiere mesas de trabajo?");

                entity.Property(e => e.EsRequiereSupervisor).HasComment("¿Está actuación requiere la participación o insumo del supervisor del contrato?");

                entity.Property(e => e.EsprocesoResultadoDefinitivo).HasComment("¿El proceso tiene resultado definitivo y se considera cerrado el proceso?");

                entity.Property(e => e.EstadoActuacionReclamacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoAvanceTramiteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoCodigoActuacionDerivada)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de actuación de la controversia");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vencimiento del registro");

                entity.Property(e => e.NumeroActuacion)
                    .HasMaxLength(100)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroActuacionReclamacion)
                    .HasMaxLength(100)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProximaActuacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ProximaActuacionOtro).HasComment("“Actuaciones de controversias contractuales” se incluye el valor “otro”.");

                entity.Property(e => e.RegistroCompletoActuacionDerivada).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.ResumenPropuestaFiduciaria).HasComment("Resumen");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta URL con soportes de la actuación");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaContractual)
                    .WithMany(p => p.ControversiaActuacion)
                    .HasForeignKey(d => d.ControversiaContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaActuacion_ControversiaContractual");
            });

            modelBuilder.Entity<ControversiaActuacionMesa>(entity =>
            {
                entity.HasComment("Almacena la mesa de trabajo relacionada a la actuación de la controversia contractual");

                entity.Property(e => e.ControversiaActuacionMesaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la actuación adelantada");

                entity.Property(e => e.CantDiasVencimiento).HasComment("Cantidad de días de vencimiento de la controversia de la actuación");

                entity.Property(e => e.ControversiaActuacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoAvanceMesaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoRegistroCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacionAdelantada)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la actuación adelantada de la controversia");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vencimiento del registro");

                entity.Property(e => e.NumeroMesaTrabajo)
                    .HasMaxLength(100)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProximaActuacionRequerida)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Parrafo de la siguiente actuación requerida");

                entity.Property(e => e.ResultadoDefinitivo).HasComment("Indica si es el resultado definitivo");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del soporte");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.ControversiaActuacionMesa)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Controver__Contr__23DE44F1");
            });

            modelBuilder.Entity<ControversiaActuacionMesaSeguimiento>(entity =>
            {
                entity.HasComment("Almacena el seguimiento a la mesa de trabajo relacionada a la actuación de la controversia");

                entity.Property(e => e.ControversiaActuacionMesaSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la actuación adelantada");

                entity.Property(e => e.CantDiasVencimiento).HasComment("Cantidad de días de vencimiento de la controversia de la actuación en el seguimiento");

                entity.Property(e => e.ControversiaActuacionMesaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoAvanceMesaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoRegistroCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacionAdelantada)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la actuación adelantada de la controversia");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vencimiento del registro");

                entity.Property(e => e.NumeroActuacionSeguimiento)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProximaActuacionRequerida)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Parrafo de la siguiente actuación requerida");

                entity.Property(e => e.ResultadoDefinitivo).HasComment("Indica si es el resultado definitivo");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del soporte");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaActuacionMesa)
                    .WithMany(p => p.ControversiaActuacionMesaSeguimiento)
                    .HasForeignKey(d => d.ControversiaActuacionMesaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Controver__Contr__2F4FF79D");
            });

            modelBuilder.Entity<ControversiaContractual>(entity =>
            {
                entity.HasComment("Almacena las controversias contractuales relacionadas a un contrato");

                entity.Property(e => e.ControversiaContractualId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConclusionComitePreTecnico)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Conclusión del Comité pre-técnico de acuerdo con la solicitud");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CualOtroMotivo)
                    .IsUnicode(false)
                    .HasComment("Motivo adicional de la controversia contratual");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EsProcede).HasComment("¿La solicitud procede?");

                entity.Property(e => e.EsRequiereComite).HasComment("¿Requiere comité técnico?");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaComitePreTecnico)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de comité pretécnico");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSolicitud)
                    .HasColumnType("datetime")
                    .HasComment("Fecha solicitud");

                entity.Property(e => e.MotivoJustificacionRechazo)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Campo que sirve para todos los tipos de Controversia (Motivos de rechazo, Resumen de la justificación de la solicitud)");

                entity.Property(e => e.NumeroRadicadoSac)
                    .HasColumnName("NumeroRadicadoSAC")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("número de radicación de la solicitud.");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("código de consecutivo de controversia con las siguientes características: CO para contratos de obra o CI para contratos de interventoría, seguido del consecutivo de la controversia 001 a 00n seguido del número del contrato.");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta URL con soportes de la solicitud");

                entity.Property(e => e.SolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoControversiaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ControversiaContractual)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaContractual_Contrato");
            });

            modelBuilder.Entity<ControversiaMotivo>(entity =>
            {
                entity.HasComment("Almacena los motivos de un controversia contractual");

                entity.Property(e => e.ControversiaMotivoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ControversiaContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MotivoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaContractual)
                    .WithMany(p => p.ControversiaMotivo)
                    .HasForeignKey(d => d.ControversiaContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaMotivo_ControversiaContractual");
            });

            modelBuilder.Entity<CriterioCodigoTipoPagoCodigo>(entity =>
            {
                entity.HasComment("Almacena los códigos de criterio del tipo de pago");

                entity.Property(e => e.CriterioCodigoTipoPagoCodigoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CriterioCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoPagoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");
            });

            modelBuilder.Entity<CronogramaSeguimiento>(entity =>
            {
                entity.HasComment("Almacena los cronogramas un proceso de selección");

                entity.Property(e => e.CronogramaSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActividadFinalCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoActividadInicialCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones del cronograma");

                entity.Property(e => e.ProcesoSeleccionCronogramaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccionCronograma)
                    .WithMany(p => p.CronogramaSeguimiento)
                    .HasForeignKey(d => d.ProcesoSeleccionCronogramaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CronogramaSeguimiento_ProcesoSeleccionCronograma");
            });

            modelBuilder.Entity<CuentaBancaria>(entity =>
            {
                entity.HasComment("Almacena las cuentas bancarias relacionadasa una fuente de financiación");

                entity.Property(e => e.CuentaBancariaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.BancoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Exenta).HasComment("Indica que la cuenta es Exenta del 4x1000");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NombreCuentaBanco)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre de Cuenta");

                entity.Property(e => e.NumeroCuentaBanco)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de cuenta");

                entity.Property(e => e.TipoCuentaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.CuentaBancaria)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_CuentaBancaria_FuenteFinanciacion");
            });

            modelBuilder.Entity<DefensaJudicial>(entity =>
            {
                entity.HasComment("Almacena los procesos de defensa judicial");

                entity.Property(e => e.DefensaJudicialId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CanalIngresoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CantContratos).HasComment("Número de contratos vinculados al proceso");

                entity.Property(e => e.CuantiaPerjuicios)
                    .HasColumnType("numeric(18, 0)")
                    .HasComment("Valor de los perjuicios");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EsDemandaFfie)
                    .HasColumnName("EsDemandaFFIE")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsLegitimacionActiva).HasComment("proceso se crea desde el FFIE como demandante (Activa) (1) o desde un tercero hacia el FFIE (Pasiva) (0)");

                entity.Property(e => e.EsRequiereSupervisor).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ExisteConocimiento).HasComment("Existe conocimiento del proceso de defensa judicial");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRadicadoFfie)
                    .HasColumnName("FechaRadicadoFFIE")
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del FFIE");

                entity.Property(e => e.InstitucionEducativaSedeId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.JurisdiccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.LegitimacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.LocalizacionIdMunicipio).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.NumeroDemandados).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroDemandantes).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroProceso)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("El número se creará de manera automática y estará compuesto por las siglas (DJ – consecutivo automático – el año de apertura del proceso)");

                entity.Property(e => e.NumeroRadicadoFfie)
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Pretensiones).HasComment("Descripción de las pretensiones");

                entity.Property(e => e.SolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoAccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlSoporteProceso)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<DefensaJudicialContratacionProyecto>(entity =>
            {
                entity.HasComment("Almacena los procesos de defensa judicial a una contratación de un proyecto");

                entity.Property(e => e.DefensaJudicialContratacionProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DefensaJudicialId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.DefensaJudicialContratacionProyecto)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefensaJudicialContratacionProyecto_ContratacionProyecto");

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.DefensaJudicialContratacionProyecto)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .HasConstraintName("FK_DefensaJudicialContratacionProyecto_DefensaJudicial");
            });

            modelBuilder.Entity<DefensaJudicialSeguimiento>(entity =>
            {
                entity.HasComment("Almacena el avance de los procesos de defensa judicial");

                entity.Property(e => e.DefensaJudicialSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActuacionAdelantada).HasComment("registrar la actividad que considere debe realizarse.");

                entity.Property(e => e.DefensaJudicialId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EsRequiereSupervisor).HasComment("¿Está actuación requiere la participación o insumo del supervisor del contrato?");

                entity.Property(e => e.EsprocesoResultadoDefinitivo).HasComment("¿El proceso tiene actuación definitiva (sentencia, o acto de autocomposición procesal -Transacción, Conciliación, Desistimiento o un convencimiento-), ¿y se considera cerrado el proceso?");

                entity.Property(e => e.EstadoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de actuación del seguimiento");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de vencimiento del registro");

                entity.Property(e => e.NumeroActuacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProximaActuacion).HasComment("registrar la actividad que considere debe realizarse");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta URL con soportes de la actuación");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.DefensaJudicialSeguimiento)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefensaJudicialSeguimiento_DefensaJudicial");
            });

            modelBuilder.Entity<DemandadoConvocado>(entity =>
            {
                entity.HasComment("Almacena los datos del ente demandado");

                entity.Property(e => e.DemandadoConvocadoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CaducidadPrescripcion)
                    .HasColumnType("date")
                    .HasComment("Caducidad o Prescripción");

                entity.Property(e => e.ConvocadoAutoridadDespacho)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Nombre del convocado");

                entity.Property(e => e.DefensaJudicialId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Dirección física para envío de notificaciones");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico para envío de notificaciones");

                entity.Property(e => e.EsConvocado).HasComment("Indica si el registro pertenece a un demandado o un convocado. (0. Demandante, 1. Convocante)");

                entity.Property(e => e.EsDemandado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EtapaProcesoFfiecodigo)
                    .HasColumnName("EtapaProcesoFFIECodigo")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ExisteConocimiento).HasComment("Existe conocimiento por parte del demandado convocado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicadodel demandado convocado");

                entity.Property(e => e.LocalizacionIdMunicipio).HasComment("identificador del Municipio de la Autoridad");

                entity.Property(e => e.MedioControlAccion)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Medio de Control / Acción a evitar");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("nombre de la persona natural o jurídica del demandante.");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("número de identificación..");

                entity.Property(e => e.RadicadoDespacho)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Radicado en despacho de conocimiento");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.DemandadoConvocado)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .HasConstraintName("FK_Demandado_DefensaJucicial");
            });

            modelBuilder.Entity<DemandanteConvocante>(entity =>
            {
                entity.HasKey(e => e.DemandanteConvocadoId);

                entity.HasComment("Almacena los datos del ente demandante");

                entity.Property(e => e.DemandanteConvocadoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantDemandados).HasComment("Número de demandados");

                entity.Property(e => e.DefensaJucicialId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Dirección física para envío de notificaciones");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico para envío de notificaciones");

                entity.Property(e => e.EsConvocante).HasComment("Indica si el registro pertenece a un demandante o un convocante. (0. Demandante, 1. Convocante)  ");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("nombre de la persona natural o jurídica del demandante.");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("número de identificación..");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.DefensaJucicial)
                    .WithMany(p => p.DemandanteConvocante)
                    .HasForeignKey(d => d.DefensaJucicialId)
                    .HasConstraintName("FK_Demandante_DefensaJucicial");
            });

            modelBuilder.Entity<DevMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Dev_Menu");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FaseCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Icono)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RutaFormulario)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DisponibilidadPresupuestal>(entity =>
            {
                entity.HasComment("Almacena las solicitudes de disponibilidad presupuestal");

                entity.Property(e => e.DisponibilidadPresupuestalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CuentaCartaAutorizacion).HasComment("¿Cuenta con carta de autorización de la ET?");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsNovedadContractual).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaDdp)
                    .HasColumnName("FechaDDP")
                    .HasColumnType("datetime")
                    .HasComment("Fecha del DDP");

                entity.Property(e => e.FechaDrp)
                    .HasColumnName("FechaDRP")
                    .HasColumnType("datetime")
                    .HasComment("Fecha del DRP");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSolicitud)
                    .HasColumnType("datetime")
                    .HasComment("fecha de la solicitud");

                entity.Property(e => e.LimitacionEspecial)
                    .HasMaxLength(4000)
                    .HasComment("Limitación Especial");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Numero asignado del contrato");

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("si la solicitud es nueva asignará un Número DDP automático que  cumpla con con la siguiente estructura: DDP_PI_autoconsecutivo");

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número DRP");

                entity.Property(e => e.NumeroRadicadoSolicitud)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Númer de solicitud");

                entity.Property(e => e.Objeto)
                    .HasMaxLength(4000)
                    .HasComment("Objeto");

                entity.Property(e => e.OpcionContratarCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RutaDdp)
                    .HasColumnName("RutaDDP")
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta o url para descargar pdf");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoSolicitudEspecialCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(3000)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAportante)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorSolicitud)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.DisponibilidadPresupuestal)
                    .HasForeignKey(d => d.AportanteId)
                    .HasConstraintName("FK_DisponibilidadPresupuestal_Aportante");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.DisponibilidadPresupuestal)
                    .HasForeignKey(d => d.ContratacionId)
                    .HasConstraintName("FK_DisponibilidadPresupuestal_Contratacion");
            });

            modelBuilder.Entity<DisponibilidadPresupuestalObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones de las solicitudes de disponibilidad presupuestal");

                entity.Property(e => e.DisponibilidadPresupuestalObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.DisponibilidadPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EsNovedad).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.NovedadContractualRegistroPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Observación asociada a la disponibilidad Presupuestal");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.DisponibilidadPresupuestal)
                    .WithMany(p => p.DisponibilidadPresupuestalObservacion)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisponibilidadPresupuestalObservacion_DisponibilidadPresupuestal");

                entity.HasOne(d => d.NovedadContractualRegistroPresupuestal)
                    .WithMany(p => p.DisponibilidadPresupuestalObservacion)
                    .HasForeignKey(d => d.NovedadContractualRegistroPresupuestalId)
                    .HasConstraintName("FK_DisponibilidadNovedadContractualRegistroPresupuestal");
            });

            modelBuilder.Entity<DisponibilidadPresupuestalProyecto>(entity =>
            {
                entity.HasComment("Almacena las relaciones de las solicitudes de disponibilidad presupuestal con los proyectos");

                entity.Property(e => e.DisponibilidadPresupuestalProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.DisponibilidadPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ProyectoAdministrativoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.DisponibilidadPresupuestal)
                    .WithMany(p => p.DisponibilidadPresupuestalProyecto)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisponibilidadPresupuestalProyecto_DisponibilidadPresupuestal1");

                entity.HasOne(d => d.ProyectoAdministrativo)
                    .WithMany(p => p.DisponibilidadPresupuestalProyecto)
                    .HasForeignKey(d => d.ProyectoAdministrativoId)
                    .HasConstraintName("FK_DisponibilidadPresupuestalProyecto_ProyectoAdministrativo");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.DisponibilidadPresupuestalProyecto)
                    .HasForeignKey(d => d.ProyectoId)
                    .HasConstraintName("FK_DisponibilidadPresupuestalProyecto_Proyecto");
            });

            modelBuilder.Entity<DocumentoApropiacion>(entity =>
            {
                entity.HasComment("Almacena los documento de apropiación relacionados al aportante");

                entity.Property(e => e.DocumentoApropiacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se hace la acción");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.NumeroDocumento)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Número de documento");

                entity.Property(e => e.TipoDocumentoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.VigenciaAporteCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.DocumentoApropiacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoApropiacion_Aportante");
            });

            modelBuilder.Entity<Dominio>(entity =>
            {
                entity.HasComment("Almacena los diferentes valores que pueden tomar las parametricas en el sistema");

                entity.Property(e => e.DominioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica que la parametrica esta activa en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .HasComment("Descripción de la  parametrica en el sistema");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasComment("Nombre del Tipo de parametrica en el sistema");

                entity.Property(e => e.TipoDominioId).HasComment("Identificador de la tabla del Tipo de dominio al que pertenece la parametrica");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.TipoDominio)
                    .WithMany(p => p.Dominio)
                    .HasForeignKey(d => d.TipoDominioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dominio_TipoDominio");
            });

            modelBuilder.Entity<EnsayoLaboratorioMuestra>(entity =>
            {
                entity.HasComment("Almacena los resultados de las muestras tomadas en las obras");

                entity.Property(e => e.EnsayoLaboratorioMuestraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEntregaResultado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de entrega de resultados");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.GestionObraCalidadEnsayoLaboratorioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NombreMuestra)
                    .HasMaxLength(40)
                    .HasComment("Nombre de la muestra");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.GestionObraCalidadEnsayoLaboratorio)
                    .WithMany(p => p.EnsayoLaboratorioMuestra)
                    .HasForeignKey(d => d.GestionObraCalidadEnsayoLaboratorioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_EnsayoLaboratorioMuestra_GestionObraCalidadEnsayoLaboratorio_1");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.EnsayoLaboratorioMuestraObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_EnsayoLaboratorioMuestra_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.EnsayoLaboratorioMuestraObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_EnsayoLaboratorioMuestra_SeguimientoSemanalObservacionSupervisor");
            });

            modelBuilder.Entity<FaseComponenteUso>(entity =>
            {
                entity.HasComment("Almacena la relación entre la fase el componente y el uso");

                entity.Property(e => e.FaseComponenteUsoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComponenteId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FaseId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la tabla en mención");
            });

            modelBuilder.Entity<FichaEstudio>(entity =>
            {
                entity.HasComment("Almacena la ficha de estudio a los procesos de defensa judicial");

                entity.Property(e => e.FichaEstudioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Abogado)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Abogado que elabora el estudio");

                entity.Property(e => e.AnalisisJuridico).HasComment("registrar el análisis jurídico del proceso, basado en el material probatorio, antecedentes, los hechos relevantes, su conocimiento y experiencia");

                entity.Property(e => e.Antecedentes).HasComment("Antecedentes");

                entity.Property(e => e.DecisionComiteDirectrices).HasComment("registrar como referencia las decisiones y directrices que se tomaron en casos anteriores");

                entity.Property(e => e.DefensaJudicialId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsActuacionTramiteComite).HasComment("¿La actuación recomendada debe surtir trámite en comité técnico?");

                entity.Property(e => e.EsAprobadoAperturaProceso).HasComment("Aplica para los casos, donde el proceso es de legitimación activa, donde el FFIE es el demandante,");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EsPresentadoAnteComiteFfie)
                    .HasColumnName("EsPresentadoAnteComiteFFIE")
                    .HasComment("¿Este proceso y su ficha se presentaron ante el Comité de Defensa del FFIE?");

                entity.Property(e => e.FechaComiteDefensa)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de comité de defensa");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.HechosRelevantes).HasComment("registrar puntualmente los hechos relevantes del proceso.");

                entity.Property(e => e.JurisprudenciaDoctrina).HasComment("registrar los conceptos");

                entity.Property(e => e.RecomendacionFinalComite).HasComment("Recomendaciones finales del comité");

                entity.Property(e => e.Recomendaciones).HasComment("registrar sus recomendaciones para el proceso, basado en el análisis jurídico.");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("URL Soporte del material probatorio, en el cual el usuario podrá diligenciar la ubicación de los documentos que sustentan la ficha de análisis jurídico y recomendaciones frente al proceso.");

                entity.Property(e => e.TipoActuacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.FichaEstudio)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaEstudio_DefensaJudicial");
            });

            modelBuilder.Entity<FlujoInversion>(entity =>
            {
                entity.HasComment("Almacena el flujo de inversión de un contrato de inversión dependienco de la programación de la ejecución");

                entity.Property(e => e.FlujoInversionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.MesEjecucionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Semana)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Número de la semana");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.FlujoInversion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FlujoInversion_ContratoConstruccion");

                entity.HasOne(d => d.MesEjecucion)
                    .WithMany(p => p.FlujoInversion)
                    .HasForeignKey(d => d.MesEjecucionId)
                    .HasConstraintName("FK_FlujoInversion_MesEjecucion");

                entity.HasOne(d => d.Programacion)
                    .WithMany(p => p.FlujoInversion)
                    .HasForeignKey(d => d.ProgramacionId)
                    .HasConstraintName("FK_FlujoInversion_Programacion");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.FlujoInversion)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .HasConstraintName("FK_FlujoInversion_SeguimientoSemanal");
            });

            modelBuilder.Entity<FormaPagoCriterioPago>(entity =>
            {
                entity.HasKey(e => e.FormaPagoCodigoCriterioPagoCodigoId)
                    .HasName("PK__FormaPag__DFCDB1CEC4ACB432");

                entity.HasComment("Almacena la relación de la forma de pago con el criterio de pago");

                entity.Property(e => e.FormaPagoCodigoCriterioPagoCodigoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CriterioPagoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FormaPagoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");
            });

            modelBuilder.Entity<FormasPagoFase>(entity =>
            {
                entity.HasComment("Almacena el código de la forma de pago en relación a la fase del proyecto");

                entity.Property(e => e.FormasPagoFaseId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.EsPreconstruccion).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FormaPagoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");
            });

            modelBuilder.Entity<FuenteFinanciacion>(entity =>
            {
                entity.HasComment("Almacena las fuestes de financiación relacionadas al aportante");

                entity.HasIndex(e => new { e.AportanteId, e.Eliminado })
                    .HasName("indexaportante");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CantVigencias).HasComment("¿De cuántas vigencias se realizará el aporte?");

                entity.Property(e => e.CofinanciacionDocumentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorFuente)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.FuenteFinanciacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FuenteFinanciacion_Aportante");

                entity.HasOne(d => d.CofinanciacionDocumento)
                    .WithMany(p => p.FuenteFinanciacion)
                    .HasForeignKey(d => d.CofinanciacionDocumentoId)
                    .HasConstraintName("FK_FuenteDicumentoFinanciacion");
            });

            modelBuilder.Entity<GestionFuenteFinanciacion>(entity =>
            {
                entity.HasComment("Almacena la gestión a la fuente de financiación");

                entity.Property(e => e.GestionFuenteFinanciacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.BalanceFinancieroTrasladoValorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DisponibilidadPresupuestalId)
                    .HasColumnName("DisponibilidadPresupuestalID")
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DisponibilidadPresupuestalProyectoId)
                    .HasColumnName("DisponibilidadPresupuestalProyectoID")
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsNovedad).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NovedadContractualRegistroPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NuevoSaldo)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("se calculará  automáticamente  y debe mostrar    FORMATO CASO DE USO    07/01/2020  Versión 1 Página 4 de 6    Cualquier copia impresa de este documento se considera como COPIA NO CONTROLADA.    la información  asociada al saldo  actual de la  fuente(s) menos  el valor solicitado  de la fuente(s).  ");

                entity.Property(e => e.NuevoSaldoGenerado)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Nuevo saldo generado");

                entity.Property(e => e.RendimientosIncorporadosId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SaldoActual)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Saldo actual de la  fuente");

                entity.Property(e => e.SaldoActualGenerado)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Valor del saldo actual generado");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorSolicitado)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorSolicitadoGenerado)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.BalanceFinancieroTrasladoValor)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.BalanceFinancieroTrasladoValorId)
                    .HasConstraintName("FK_BalanceFinancieroTrasladoValorId");

                entity.HasOne(d => d.DisponibilidadPresupuestal)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalId)
                    .HasConstraintName("FK_DisponibilidadPresupuestal");

                entity.HasOne(d => d.DisponibilidadPresupuestalProyecto)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalProyectoId)
                    .HasConstraintName("FK_DiponibiliddadPP");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GestionFuenteFinanciacion_FuenteFinanciacion");

                entity.HasOne(d => d.RendimientosIncorporados)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.RendimientosIncorporadosId)
                    .HasConstraintName("FK_GestionFuenteFinanciacion_RendimientosIncorporados");
            });

            modelBuilder.Entity<GestionObraCalidadEnsayoLaboratorio>(entity =>
            {
                entity.HasComment("Almacena el tipo de ensayo y/o muestra al seguimiento semanal de la gestión de la obra");

                entity.Property(e => e.GestionObraCalidadEnsayoLaboratorioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEntregaResultados)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de entrega de resultados");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaTomaMuestras)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de toma de muestras");

                entity.Property(e => e.NumeroMuestras).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RealizoControlMedicion).HasComment("Índica si realizo el control de medición");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoMuestras).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeguimientoSemanalGestionObraCalidadId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoEnsayoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlSoporteGestion).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.GestionObraCalidadEnsayoLaboratorioObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_GestionObraCalidadEnsayoLaboratorio_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.GestionObraCalidadEnsayoLaboratorioObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_GestionObraCalidadEnsayoLaboratorio_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObraCalidad)
                    .WithMany(p => p.GestionObraCalidadEnsayoLaboratorio)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraCalidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_GestionObraCalidadEnsayoLaboratorio_SeguimientoSemanalGestionObraCalidad_1");
            });

            modelBuilder.Entity<GrupoMunicipios>(entity =>
            {
                entity.HasComment("Almacena los municipios agregados a un proceso de selección");

                entity.Property(e => e.GrupoMunicipiosId)
                    .HasComment("Llave foranea a la tabla en mención")
                    .ValueGeneratedNever();

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.LocalizacionIdMunicipio).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.ProcesoSeleccionGrupoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccionGrupo)
                    .WithMany(p => p.GrupoMunicipios)
                    .HasForeignKey(d => d.ProcesoSeleccionGrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GrupoMunicipios_ProcesoSeleccionGrupo");
            });

            modelBuilder.Entity<IndicadorReporte>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Almacena los reportes e indicadores de PowerBI");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.Etapa)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Indica el número de la tapa");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Llave del grupo en PowerBI");

                entity.Property(e => e.Indicador).HasComment("indica si es un indicador o un reporte");

                entity.Property(e => e.IndicadorReporteId)
                    .HasComment("Llave primaria de la tabla")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Nombre del reporte indicador");

                entity.Property(e => e.Proceso)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Proceso al que pertenece el indicador de reportes");

                entity.Property(e => e.ReportId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Llave de reporte en PowerBI");
            });

            modelBuilder.Entity<InformeFinal>(entity =>
            {
                entity.HasComment("Almacena los informes finales relacionados a un proyecto");

                entity.Property(e => e.InformeFinalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoAprobacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado de la aprobación del informe final");

                entity.Property(e => e.EstadoCumplimiento)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado del cumplimiento del informe final");

                entity.Property(e => e.EstadoEntregaEtc)
                    .HasColumnName("EstadoEntregaETC")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Indica el estado de la entrega a la ETC");

                entity.Property(e => e.EstadoInforme)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica el estado del informe");

                entity.Property(e => e.EstadoValidacion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica el estado de la validación del informe final");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaAprobacionFinal)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación final");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEnvioApoyoSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del informe final a apoyo a la supervisión");

                entity.Property(e => e.FechaEnvioEtc)
                    .HasColumnName("FechaEnvioETC")
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del informe final a la ETC");

                entity.Property(e => e.FechaEnvioGrupoNovedades)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del informe final al grupo de novedades");

                entity.Property(e => e.FechaEnvioSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del informe final al supervisor");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSuscripcion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de suscripción del inform final");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoCumplimiento).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoEntregaEtc)
                    .HasColumnName("RegistroCompletoEntregaETC")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoValidacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionesCumplimiento).HasComment("Campo que indica que tiene observaciones de cumplimiento");

                entity.Property(e => e.TieneObservacionesInterventoria).HasComment("Campo que indica que tiene observaciones de interventoria");

                entity.Property(e => e.TieneObservacionesSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.TieneObservacionesValidacion).HasComment("Campo que indica que tiene observaciones de validación");

                entity.Property(e => e.UrlActa)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.InformeFinal)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_informe_final_proyecto");
            });

            modelBuilder.Entity<InformeFinalAnexo>(entity =>
            {
                entity.HasComment("Almacena los anexos de los informes finales relacionados a un proyecto");

                entity.Property(e => e.InformeFinalAnexoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado del anexo del informe final");

                entity.Property(e => e.NumRadicadoSac)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número de radicado SAC");

                entity.Property(e => e.TipoAnexo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Tipo del anexo");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<InformeFinalInterventoria>(entity =>
            {
                entity.HasComment("Almacena la gestión de la interventoria frente al informe final");

                entity.Property(e => e.InformeFinalInterventoriaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AprobacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CalificacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.InformeFinalAnexoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.InformeFinalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.InformeFinalListaChequeoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneModificacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneModificacionInterventor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValidacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.HasOne(d => d.InformeFinalAnexo)
                    .WithMany(p => p.InformeFinalInterventoria)
                    .HasForeignKey(d => d.InformeFinalAnexoId)
                    .HasConstraintName("FK_InformeFinalAnexo");

                entity.HasOne(d => d.InformeFinal)
                    .WithMany(p => p.InformeFinalInterventoria)
                    .HasForeignKey(d => d.InformeFinalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__InformeFinalInterventoria");

                entity.HasOne(d => d.InformeFinalListaChequeo)
                    .WithMany(p => p.InformeFinalInterventoria)
                    .HasForeignKey(d => d.InformeFinalListaChequeoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InformeFinalInterventoria_ListaChequeo");
            });

            modelBuilder.Entity<InformeFinalInterventoriaObservaciones>(entity =>
            {
                entity.HasComment("Almacena las observaciones de la interventoria en un informe final");

                entity.Property(e => e.InformeFinalInterventoriaObservacionesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsApoyo).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsCalificacion).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.InformeFinalInterventoriaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.InformeFinalInterventoria)
                    .WithMany(p => p.InformeFinalInterventoriaObservaciones)
                    .HasForeignKey(d => d.InformeFinalInterventoriaId)
                    .HasConstraintName("FK__InformeFinalInterventoriaObservacion");
            });

            modelBuilder.Entity<InformeFinalListaChequeo>(entity =>
            {
                entity.HasComment("Almacena la lista de chequeo relacionada a un informe final");

                entity.Property(e => e.InformeFinalListaChequeoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MensajeAyuda)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Mensaje ayuda");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Nombre de la lista de chequeo del informe final");

                entity.Property(e => e.Posicion).HasComment("Posición en la lista de chequeo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<InformeFinalObservaciones>(entity =>
            {
                entity.HasComment("Almacena las observaciones de un informa final");

                entity.Property(e => e.InformeFinalObservacionesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsApoyo).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsGrupoNovedades).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsGrupoNovedadesInterventoria).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.InformeFinalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.InformeFinal)
                    .WithMany(p => p.InformeFinalObservaciones)
                    .HasForeignKey(d => d.InformeFinalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__InformeFinalObservacion");
            });

            modelBuilder.Entity<InfraestructuraIntervenirProyecto>(entity =>
            {
                entity.HasKey(e => e.InfraestrucutraIntervenirProyectoId);

                entity.HasComment("Almacena la infraestructura a intervenir en relación a un proyecto");

                entity.Property(e => e.InfraestrucutraIntervenirProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Cantidad).HasComment("Cantidad de unidades");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEliminacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de eliminación");

                entity.Property(e => e.InfraestructuraCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioEliminacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que elimino el registro del proyecto");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.InfraestructuraIntervenirProyecto)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InfraestructuraIntervenirProyecto_Proyecto");
            });

            modelBuilder.Entity<InstitucionEducativaSede>(entity =>
            {
                entity.HasComment("Almacena todas las instituciones educativas");

                entity.Property(e => e.InstitucionEducativaSedeId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.CodigoDane)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Nombre de la Institución Educativa");

                entity.Property(e => e.PadreId).HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");
            });

            modelBuilder.Entity<LiquidacionContratacionObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones sobre una liquidación contratación");

                entity.Property(e => e.LiquidacionContratacionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.ContratacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.IdPadre).HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.MenuId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TieneObservacion).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.LiquidacionContratacionObservacion)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LiquidacionContratacionObservacion_Contratacion");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.LiquidacionContratacionObservacion)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_LiquidacionContratacionObservacion_Menu");
            });

            modelBuilder.Entity<ListaChequeo>(entity =>
            {
                entity.HasComment("Almacena las listas de chequeo del sistema");

                entity.Property(e => e.ListaChequeoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.CriterioPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsObra).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoMenuCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Nombre de la lista de chequeo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<ListaChequeoItem>(entity =>
            {
                entity.HasComment("Almacena los items que conforman la lista de chequeo");

                entity.Property(e => e.ListaChequeoItemId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(1500)
                    .HasComment("Nombre del item de la lista de chequeo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<ListaChequeoListaChequeoItem>(entity =>
            {
                entity.HasComment("Almacena la lista de chequeo con el item");

                entity.HasIndex(e => new { e.ListaChequeoId, e.ListaChequeoItemId })
                    .HasName("Index_ListaChequeo_ListaChequeoItem")
                    .IsUnique();

                entity.Property(e => e.ListaChequeoListaChequeoItemId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ListaChequeoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ListaChequeoItemId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Mensaje)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Mensaje");

                entity.Property(e => e.Orden).HasComment("Orden de visualización");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ListaChequeo)
                    .WithMany(p => p.ListaChequeoListaChequeoItem)
                    .HasForeignKey(d => d.ListaChequeoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListaChequeoListaChequeoItem_ListaChequeo");

                entity.HasOne(d => d.ListaChequeoItem)
                    .WithMany(p => p.ListaChequeoListaChequeoItem)
                    .HasForeignKey(d => d.ListaChequeoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListaChequeoListaChequeoItem_ListaChequeoItem");
            });

            modelBuilder.Entity<Localizacion>(entity =>
            {
                entity.HasComment("Almacena los departamentos y municipios");

                entity.Property(e => e.LocalizacionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Identificador de la tabla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Nombre de la localización");

                entity.Property(e => e.IdPadre)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.Nivel)
                    .HasColumnType("numeric(2, 0)")
                    .HasComment("Nivel al que pertenece la Localización");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Tipo de Localización");
            });

            modelBuilder.Entity<ManejoMaterialesInsumos>(entity =>
            {
                entity.HasComment("Almacena el manejo de los materiales");

                entity.Property(e => e.ManejoMaterialesInsumosId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstanProtegidosDemarcadosMateriales).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereObservacion).HasComment("Indica si requiere observación");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.Url).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.ManejoMaterialesInsumosObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_ManejoMaterialesInsumos_SeguimientoSemanalApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.ManejoMaterialesInsumosObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_ManejoMaterialesInsumos_SeguimientoSemanalSupervisor");
            });

            modelBuilder.Entity<ManejoMaterialesInsumosProveedor>(entity =>
            {
                entity.HasComment("Almacena la tendencia del manejo de los materiales con el proveedor");

                entity.Property(e => e.ManejoMaterialesInsumosProveedorId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ManejoMaterialesInsumosId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Proveedor)
                    .HasMaxLength(100)
                    .HasComment("Nombre del proveedor");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RequierePermisosAmbientalesMineros).HasComment("Indica si requiere permisos ambientales mineros");

                entity.Property(e => e.UrlRegistroFotografico)
                    .HasMaxLength(500)
                    .IsFixedLength()
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ManejoMaterialesInsumos)
                    .WithMany(p => p.ManejoMaterialesInsumosProveedor)
                    .HasForeignKey(d => d.ManejoMaterialesInsumosId)
                    .HasConstraintName("fk_ManejoMaterialesInsumosProveedor_ManejoMaterialesInsumosId_1");
            });

            modelBuilder.Entity<ManejoOtro>(entity =>
            {
                entity.HasComment("Almacena el manejo de otros materiales");

                entity.Property(e => e.ManejoOtroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Actividad).HasComment("Descripción de la actividad");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaActividad)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la actividad");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteGestion).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.ManejoOtroObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_ManejoOtro_SeguimientoSemanalApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.ManejoOtroObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_ManejoOtro_SeguimientoSemanalSupervisor");
            });

            modelBuilder.Entity<ManejoResiduosConstruccionDemolicion>(entity =>
            {
                entity.HasComment("Almacena el manejo de los residuos de demolición de construcción");

                entity.Property(e => e.ManejoResiduosConstruccionDemolicionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadToneladas).HasComment("Cantidad de toneladas");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstaCuantificadoRcd)
                    .HasColumnName("EstaCuantificadoRCD")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereObservacion).HasComment("Indica si requiere observación");

                entity.Property(e => e.SeReutilizadorResiduos).HasComment("Indica si se reutilizaron residuos");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.ManejoResiduosConstruccionDemolicionObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_ManejoResiduosConstruccionDemolicion_SeguimientoSemanalApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.ManejoResiduosConstruccionDemolicionObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_ManejoResiduosConstruccionDemolicion_SeguimientoSemanalSupervisor");
            });

            modelBuilder.Entity<ManejoResiduosConstruccionDemolicionGestor>(entity =>
            {
                entity.HasComment("Almacena los gestores del manejo de residuos");

                entity.Property(e => e.ManejoResiduosConstruccionDemolicionGestorId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ManejoResiduosConstruccionDemolicionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NombreGestorResiduos)
                    .HasMaxLength(255)
                    .HasComment("Nombre el gestor de residuos");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TienePermisoAmbiental).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.Url).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ManejoResiduosConstruccionDemolicion)
                    .WithMany(p => p.ManejoResiduosConstruccionDemolicionGestor)
                    .HasForeignKey(d => d.ManejoResiduosConstruccionDemolicionId)
                    .HasConstraintName("fk_ManejoResiduosConstruccionDemolicionGestor_ManejoResiduosConstruccionDemolicion_1");
            });

            modelBuilder.Entity<ManejoResiduosPeligrososEspeciales>(entity =>
            {
                entity.HasComment("Almacena el manejo de los residuos peligrosos especiales");

                entity.Property(e => e.ManejoResiduosPeligrososEspecialesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstanClasificados).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereObservacion).HasComment("Indica si requiere observación");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlRegistroFotografico)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.ManejoResiduosPeligrososEspecialesObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_ManejoResiduosPeligrososEspeciales_SeguimientoSemanalApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.ManejoResiduosPeligrososEspecialesObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_ManejoResiduosPeligrososEspeciales_SeguimientoSemanalSupervisor");
            });

            modelBuilder.Entity<MensajesValidaciones>(entity =>
            {
                entity.HasComment("Almacena los mensajes del sistema según el menú");

                entity.Property(e => e.MensajesValidacionesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica que la parametrica esta activa en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasComment("Mensaje Validacion");

                entity.Property(e => e.MenuId).HasComment("Modulo al que pertenecen las validaciones");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MensajesValidaciones)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("Fk_MensajesValidaciones_MenuId_Fk_Menu_MenuId");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasComment("Almacena los menús que conforman el sistema");

                entity.Property(e => e.MenuId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Descripción del Menu en el sistema");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FaseCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Icono)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Icono");

                entity.Property(e => e.MenuPadreId).HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del Menu en el sistema");

                entity.Property(e => e.Posicion).HasComment("Posición del Menu según el padre");

                entity.Property(e => e.RutaFormulario)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Ruta del Formulario");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<MenuPerfil>(entity =>
            {
                entity.HasComment("Almacena la relación de los perfiles  con el menú");

                entity.Property(e => e.MenuPerfilId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.MenuId).HasComment("Identificador del Menú");

                entity.Property(e => e.PerfilId).HasComment("Identificador del PerfilId");

                entity.Property(e => e.TienePermisoCrear)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el perfil tiene permisos de CRUD en la funcionalidad");

                entity.Property(e => e.TienePermisoEditar).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TienePermisoEliminar).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TienePermisoLeer).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuPerfil)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuPerfil_Menu");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.MenuPerfil)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuPerfil_Perfil");
            });

            modelBuilder.Entity<MesEjecucion>(entity =>
            {
                entity.HasComment("Almacena los meses de ejecución relacionado al contrato construcción");

                entity.Property(e => e.MesEjecucionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de la actividad");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasComment("Fecha inicio de la actividad");

                entity.Property(e => e.Numero).HasComment("Número del mes de ejecución");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.MesEjecucion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MesEjecucion_ContratoConstruccion");
            });

            modelBuilder.Entity<ModificacionContractual>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Almacena las modificaciones contractuales");

                entity.Property(e => e.FechaEnvioTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del trámite");

                entity.Property(e => e.FechaTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.ModificacionContractualId)
                    .HasComment("Llave primaria de la tabla")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UrlMinuta).HasComment("URL donde se encuentra el campo en mención");

                entity.HasOne(d => d.ModificacionContractualNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.ModificacionContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModificacionContractual_NovedadContractual");
            });

            modelBuilder.Entity<NovedadContractual>(entity =>
            {
                entity.HasComment("Almacena las novedades contractuales");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AbogadoRevisionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CausaRechazo)
                    .IsUnicode(false)
                    .HasComment("Causa del rechazo");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DeseaContinuar).HasComment("Campo utilizado para marcar si desea continuar");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAplicadaAcontrato)
                    .HasColumnName("EsAplicadaAContrato")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoProcesoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacionGestionContractual)
                    .HasColumnType("date")
                    .HasComment("Fecha de aprobación de la gestión contractual");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEnvioActaApoyo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del acta al apoyo de la supervisión");

                entity.Property(e => e.FechaEnvioActaContratistaInterventoria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del acta a contratista de interventoria");

                entity.Property(e => e.FechaEnvioActaContratistaObra)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio del acta a contratista de obra");

                entity.Property(e => e.FechaEnvioActaSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio  del acta al supervisor");

                entity.Property(e => e.FechaEnvioFirmaContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio para firma de contratista");

                entity.Property(e => e.FechaEnvioFirmaFiduciaria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio  para firma de fiduciaria");

                entity.Property(e => e.FechaEnvioGestionContractual)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio para gestión contractual");

                entity.Property(e => e.FechaFirmaActaContratistaObra)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de acta del contratista de obra");

                entity.Property(e => e.FechaFirmaApoyo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de apoyo a supervisión");

                entity.Property(e => e.FechaFirmaContratista)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del contratista");

                entity.Property(e => e.FechaFirmaContratistaInterventoria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de acta del contratista de interventoria");

                entity.Property(e => e.FechaFirmaFiduciaria)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de la fiduciaria del contrato");

                entity.Property(e => e.FechaFirmaSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma de la supervisión");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSesionInstancia)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la instancia de la sesión");

                entity.Property(e => e.FechaSolictud)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de solicitud");

                entity.Property(e => e.FechaTramiteGestionar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.FechaValidacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de validación");

                entity.Property(e => e.FechaVerificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de verificación");

                entity.Property(e => e.InstanciaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.NumeroOtroSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de solicitud.");

                entity.Property(e => e.ObervacionSupervisorId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionGestionar).HasComment("Observaciones al gestionar");

                entity.Property(e => e.ObservacionesDevolucionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionesTramite).HasComment("Observaciones de trámite de la novedad contractual");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RazonesNoContinuaProceso)
                    .IsUnicode(false)
                    .HasComment("Razones por las cuales no continua el proceso");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoGestionar).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoTramite).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoTramiteNovedades).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoValidacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionesApoyo).HasComment("Campo que indica que tiene observaciones del apoyo a la supervisión");

                entity.Property(e => e.TieneObservacionesSupervisor).HasComment("Campo que indica que tiene observaciones del supervisor");

                entity.Property(e => e.UrlDocumentoSuscrita)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UrlSoporteFirmas)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UrlSoporteGestionar).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.NovedadContractual)
                    .HasForeignKey(d => d.ContratoId)
                    .HasConstraintName("FK_NovedadContractual_Contrato");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.NovedadContractual)
                    .HasForeignKey(d => d.ProyectoId)
                    .HasConstraintName("FK_NovedadContractual_Proyecto");
            });

            modelBuilder.Entity<NovedadContractualAportante>(entity =>
            {
                entity.HasComment("Almacena la relación novedad contractrual con el aportante del acuerdo de cofinanciación");

                entity.Property(e => e.NovedadContractualAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CofinanciacionAportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAporte)
                    .HasColumnType("numeric(18, 9)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.CofinanciacionAportante)
                    .WithMany(p => p.NovedadContractualAportante)
                    .HasForeignKey(d => d.CofinanciacionAportanteId)
                    .HasConstraintName("FK_NovedadContractualAportante_CofinanciacionAportante");

                entity.HasOne(d => d.NovedadContractual)
                    .WithMany(p => p.NovedadContractualAportante)
                    .HasForeignKey(d => d.NovedadContractualId)
                    .HasConstraintName("FK_NovedadContractualAportante_NovedadContractual");
            });

            modelBuilder.Entity<NovedadContractualClausula>(entity =>
            {
                entity.HasComment("Almacena las clausulas sobre la novedad contractual");

                entity.Property(e => e.NovedadContractualClausulaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AjusteSolicitadoAclausula)
                    .HasColumnName("AjusteSolicitadoAClausula")
                    .HasComment("Ajuste solicitado a la clausula de la novedad contractual");

                entity.Property(e => e.ClausulaAmodificar)
                    .HasColumnName("ClausulaAModificar")
                    .HasComment("Parrafo a modificar de la clausula");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NovedadContractualDescripcionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.NovedadContractualDescripcion)
                    .WithMany(p => p.NovedadContractualClausula)
                    .HasForeignKey(d => d.NovedadContractualDescripcionId)
                    .HasConstraintName("FK_NovedadContractualClausula_NovedadContractual");
            });

            modelBuilder.Entity<NovedadContractualDescripcion>(entity =>
            {
                entity.HasComment("Almacena la descripción de la novedad contractual");

                entity.Property(e => e.NovedadContractualDescripcionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AjusteClausula)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Ajuste a la clausula de la novedad contractual");

                entity.Property(e => e.ClausulaModificar)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Parrafo a modificar de la clausula");

                entity.Property(e => e.ConceptoTecnico).HasComment("Concepto de comité técnico");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsDocumentacionSoporte).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaConcepto)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en la que se emite el concepto");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaFinSuspension)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de suspensión");

                entity.Property(e => e.FechaInicioSuspension)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de inicio de suspensión");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MotivoNovedadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NumeroRadicado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.PlazoAdicionalDias)
                    .HasColumnType("numeric(22, 2)")
                    .HasComment("Plazo de días adicionales");

                entity.Property(e => e.PlazoAdicionalMeses)
                    .HasColumnType("numeric(22, 2)")
                    .HasComment("Plazo de meses adicionales");

                entity.Property(e => e.PresupuestoAdicionalSolicitado)
                    .HasColumnType("numeric(22, 2)")
                    .HasComment("Valor del presupuesto adicional");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.ResumenJustificacion).HasComment("Resumen de la justificación");

                entity.Property(e => e.TipoNovedadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.NovedadContractual)
                    .WithMany(p => p.NovedadContractualDescripcion)
                    .HasForeignKey(d => d.NovedadContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NovedadContractualDescripcion_NovedadContractual");
            });

            modelBuilder.Entity<NovedadContractualDescripcionMotivo>(entity =>
            {
                entity.HasComment("Almacena el motivo de la descripción de la novedad contractual");

                entity.Property(e => e.NovedadContractualDescripcionMotivoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MotivoNovedadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.NovedadContractualDescripcionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.NovedadContractualDescripcion)
                    .WithMany(p => p.NovedadContractualDescripcionMotivo)
                    .HasForeignKey(d => d.NovedadContractualDescripcionId)
                    .HasConstraintName("FK_NovedadContractualDescripcionMotivo_NovedadContractualDescripcion");
            });

            modelBuilder.Entity<NovedadContractualObservaciones>(entity =>
            {
                entity.HasComment("Almacena las observaciones de la novedad contractual");

                entity.Property(e => e.NovedadContractualObservacionesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsTramiteNovedades).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.NovedadContractual)
                    .WithMany(p => p.NovedadContractualObservaciones)
                    .HasForeignKey(d => d.NovedadContractualId)
                    .HasConstraintName("FK_NovedadContractualObservaciones_NovedadContractual");
            });

            modelBuilder.Entity<NovedadContractualRegistroPresupuestal>(entity =>
            {
                entity.HasComment("Almacena la relación de la novedad contractual con el registro presupuestal");

                entity.Property(e => e.NovedadContractualRegistroPresupuestalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.DisponibilidadPresupuestalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaDdp)
                    .HasColumnName("FechaDDP")
                    .HasColumnType("datetime")
                    .HasComment("Fecha del DDP");

                entity.Property(e => e.FechaDrp)
                    .HasColumnName("FechaDRP")
                    .HasColumnType("datetime")
                    .HasComment("Fecha del DRP");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NovedadContractualId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Objeto)
                    .HasMaxLength(4000)
                    .HasComment("Objeto de la novedad contractual");

                entity.Property(e => e.PlazoDias).HasComment("Plazo en días");

                entity.Property(e => e.PlazoMeses).HasComment("Plazo en meses");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorSolicitud)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.DisponibilidadPresupuestal)
                    .WithMany(p => p.NovedadContractualRegistroPresupuestal)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalId)
                    .HasConstraintName("FK_NovedadContractualRegistroPresupuestal_DisponibilidadPresupuestal");

                entity.HasOne(d => d.NovedadContractual)
                    .WithMany(p => p.NovedadContractualRegistroPresupuestal)
                    .HasForeignKey(d => d.NovedadContractualId)
                    .HasConstraintName("FK_NovedadContractualRegistroPresupuestal_NovedadContractual");
            });

            modelBuilder.Entity<OrdenGiro>(entity =>
            {
                entity.HasComment("Almacena las ordenes de giro");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConsecutivoOrigen)
                    .HasMaxLength(50)
                    .HasComment("Identificador del origen");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRegistroCompleto)
                    .HasColumnType("datetime")
                    .HasComment("Fecha cuando el registro quedo completo");

                entity.Property(e => e.FechaRegistroCompletoAprobar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha cuando el registro quedo completo al aprobar");

                entity.Property(e => e.FechaRegistroCompletoTramitar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha cuando el registro quedo completo al tramitar");

                entity.Property(e => e.FechaRegistroCompletoVerificar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha cuando el registro quedo completo al verificar");

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.RegistroCompleto)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoAprobar)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoTramitar)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificar)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacion)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneTraslado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteFirmadoAprobar)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UrlSoporteFirmadoVerificar)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorNetoGiro)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorNetoGiroTraslado)
                    .HasColumnType("numeric(38, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");
            });

            modelBuilder.Entity<OrdenGiroDetalle>(entity =>
            {
                entity.HasComment("Almacena el detalle de las ordenes de giro");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.OrdenGiroDetalle)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .HasConstraintName("FK_OrdenGiroDetalle_OrdenGiro");
            });

            modelBuilder.Entity<OrdenGiroDetalleDescuentoTecnica>(entity =>
            {
                entity.HasComment("Almacena el descuento técnico de las ordenes de giro");

                entity.Property(e => e.OrdenGiroDetalleDescuentoTecnicaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsPreconstruccion)
                    .HasColumnName("esPreconstruccion")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.SolicitudPagoFaseFacturaDescuentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroDetalle)
                    .WithMany(p => p.OrdenGiroDetalleDescuentoTecnica)
                    .HasForeignKey(d => d.OrdenGiroDetalleId)
                    .HasConstraintName("FK_OrdenGiroDetalleDescuentoTecnica_OrdenGiroDetalle");
            });

            modelBuilder.Entity<OrdenGiroDetalleDescuentoTecnicaAportante>(entity =>
            {
                entity.HasComment("Almacena la relación entre el aportante y el descuento técnico de la ordend e giro");

                entity.Property(e => e.OrdenGiroDetalleDescuentoTecnicaAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.OrdenGiroDetalleDescuentoTecnicaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RequiereDescuento).HasComment("Indica si requiere descuento");

                entity.Property(e => e.SolicitudPagoFaseFacturaDescuentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorDescuento).HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.OrdenGiroDetalleDescuentoTecnicaAportante)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_OrdenGiroDetalleDescuentoTecnicaAportante_FuenteFinanciacion");

                entity.HasOne(d => d.OrdenGiroDetalleDescuentoTecnica)
                    .WithMany(p => p.OrdenGiroDetalleDescuentoTecnicaAportante)
                    .HasForeignKey(d => d.OrdenGiroDetalleDescuentoTecnicaId)
                    .HasConstraintName("FK_OrdenGiroDetalleDescuentoTecnicaAportante_OrdenGiroDetalleDescuentoTecnica");
            });

            modelBuilder.Entity<OrdenGiroDetalleEstrategiaPago>(entity =>
            {
                entity.HasComment("Almacena la estrategia de pago de una orden giro");

                entity.Property(e => e.OrdenGiroDetalleEstrategiaPagoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstrategiaPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroDetalle)
                    .WithMany(p => p.OrdenGiroDetalleEstrategiaPago)
                    .HasForeignKey(d => d.OrdenGiroDetalleId)
                    .HasConstraintName("FK_OrdenGiroDetalleEstrategiaPago_OrdenGiroDetalle");
            });

            modelBuilder.Entity<OrdenGiroDetalleObservacion>(entity =>
            {
                entity.HasKey(e => e.OrdenGiroObservacionId)
                    .HasName("PK__OrdenGir__C509FDB5161AAC15");

                entity.HasComment("Almacena las observaciones de una orden de giro");

                entity.Property(e => e.OrdenGiroObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroDetalle)
                    .WithMany(p => p.OrdenGiroDetalleObservacion)
                    .HasForeignKey(d => d.OrdenGiroDetalleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenGiroDetalleObservacion_OrdenGiro");
            });

            modelBuilder.Entity<OrdenGiroDetalleTerceroCausacion>(entity =>
            {
                entity.HasComment("Almacena la causación de una orden de giro");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ConceptoPagoCriterio)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Criterio del concepto de pago");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsPreconstruccion)
                    .HasColumnName("esPreconstruccion")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoOrigen).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneDescuento).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorFacturadoConcepto).HasColumnType("decimal(25, 3)");

                entity.Property(e => e.ValorNetoGiro)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.OrdenGiroDetalle)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacion)
                    .HasForeignKey(d => d.OrdenGiroDetalleId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacion_OrdenGiroDetalle");
            });

            modelBuilder.Entity<OrdenGiroDetalleTerceroCausacionAportante>(entity =>
            {
                entity.HasComment("Almacena la relación de los aportantes frente a la causación de una orden de giro");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CuentaBancariaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FuenteRecursoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoOrigen).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorDescuento)
                    .HasColumnType("decimal(38, 0)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionAportante)
                    .HasForeignKey(d => d.AportanteId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionAportante_Aportante");

                entity.HasOne(d => d.CuentaBancaria)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionAportante)
                    .HasForeignKey(d => d.CuentaBancariaId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionAportante_CuentaBancaria");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionAportante)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionAportanteFuenteFinanciacion");

                entity.HasOne(d => d.OrdenGiroDetalleTerceroCausacion)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionAportante)
                    .HasForeignKey(d => d.OrdenGiroDetalleTerceroCausacionId)
                    .HasConstraintName("FK__OrdenGiroDetalleTerceroCausacionAportante_OrdenGiroDetalleTerceroCausacion");
            });

            modelBuilder.Entity<OrdenGiroDetalleTerceroCausacionDescuento>(entity =>
            {
                entity.HasComment("Almacena los descuentosde un aportante frente a una causación de una orden de giro");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionDescuentoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.OrdenGiroDetalleTerceroCausacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorDescuento)
                    .HasColumnType("decimal(25, 0)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionDescuento)
                    .HasForeignKey(d => d.AportanteId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionDescuento_AportanteId");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionDescuento)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionDescuento_FuenteFinanciacion");

                entity.HasOne(d => d.OrdenGiroDetalleTerceroCausacion)
                    .WithMany(p => p.OrdenGiroDetalleTerceroCausacionDescuento)
                    .HasForeignKey(d => d.OrdenGiroDetalleTerceroCausacionId)
                    .HasConstraintName("FK_OrdenGiroDetalleTerceroCausacionDescuento_OrdenGiroDetalleTerceroCausacion");
            });

            modelBuilder.Entity<OrdenGiroObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones de la orden de giro");

                entity.Property(e => e.OrdenGiroObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.IdPadre).HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.MenuId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TieneObservacion).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.OrdenGiroObservacion)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_OrdenGiroObservacion_Menu");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.OrdenGiroObservacion)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenGiroObservacion_OrdenGiro");
            });

            modelBuilder.Entity<OrdenGiroPago>(entity =>
            {
                entity.HasComment("Almacena la relación de la orden del giro con el pago");

                entity.Property(e => e.OrdenGiroPagoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroPagoId).HasComment("Llave foranea a la tabla en mención");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.OrdenGiroPago)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdenGiro_OrdenGiroPago");

                entity.HasOne(d => d.RegistroPago)
                    .WithMany(p => p.OrdenGiroPago)
                    .HasForeignKey(d => d.RegistroPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroPago_OrdenGiroPago");
            });

            modelBuilder.Entity<OrdenGiroSoporte>(entity =>
            {
                entity.HasComment("Almacena los soportes de una orden de giro");

                entity.Property(e => e.OrdenGiroSoporteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.OrdenGiroDetalleId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(1000)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroDetalle)
                    .WithMany(p => p.OrdenGiroSoporte)
                    .HasForeignKey(d => d.OrdenGiroDetalleId)
                    .HasConstraintName("FK_OrdenGiroSoporte_OrdenGiro");
            });

            modelBuilder.Entity<OrdenGiroTercero>(entity =>
            {
                entity.HasComment("Almacena los pagos a terceros de una orden de giro");

                entity.Property(e => e.OrdenGiroTerceroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MedioPagoGiroCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.OrdenGiroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.OrdenGiroTercero)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .HasConstraintName("OrdenGiroTercero_OrdenGiro");
            });

            modelBuilder.Entity<OrdenGiroTerceroChequeGerencia>(entity =>
            {
                entity.HasComment("Almacena los cheques gerencia de un tercero para una orden de giro");

                entity.Property(e => e.OrdenGiroTerceroChequeGerenciaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NombreBeneficiario)
                    .HasMaxLength(100)
                    .HasComment("Nombre de la persona beneficiaria del cheque");

                entity.Property(e => e.NumeroIdentificacionBeneficiario)
                    .HasMaxLength(50)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.OrdenGiroTerceroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroTercero)
                    .WithMany(p => p.OrdenGiroTerceroChequeGerencia)
                    .HasForeignKey(d => d.OrdenGiroTerceroId)
                    .HasConstraintName("FK_OrdenGiroTerceroChequeGerencia_OrdenGiroTercero");
            });

            modelBuilder.Entity<OrdenGiroTerceroTransferenciaElectronica>(entity =>
            {
                entity.HasComment("Almacena las transferencias electronicas de un tercero para una orden de giro");

                entity.Property(e => e.OrdenGiroTerceroTransferenciaElectronicaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.BancoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCuentaAhorros).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.OrdenGiroTerceroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TitularCuenta)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Titular de la cuenta del tercero para transferencia electronica");

                entity.Property(e => e.TitularNumeroIdentificacion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número de identificación del titular");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.OrdenGiroTercero)
                    .WithMany(p => p.OrdenGiroTerceroTransferenciaElectronica)
                    .HasForeignKey(d => d.OrdenGiroTerceroId)
                    .HasConstraintName("FK_OrdenGiroTerceroTransferenciaElectronica_OrdenGiroTercero");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasComment("Almacena los perfiles");

                entity.Property(e => e.PerfilId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del perfil de usuario en el sistema");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<PlanesProgramasListaChequeoRespuesta>(entity =>
            {
                entity.HasComment("Almacena las respuestas a los items de la lista de chequeo");

                entity.Property(e => e.PlanesProgramasListaChequeoRespuestaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRadicado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de radicado");

                entity.Property(e => e.ListaChequeoItemId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RecibioRequisitoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TieneObservaciones).HasComment("Campo que indica que tiene observaciones de la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ListaChequeoItem)
                    .WithMany(p => p.PlanesProgramasListaChequeoRespuesta)
                    .HasForeignKey(d => d.ListaChequeoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_PlanesProgramasListaChequeoRespuesta_ListaChequeoItem");
            });

            modelBuilder.Entity<Plantilla>(entity =>
            {
                entity.HasComment("Almacena las plantillas para formar los diferentes pdf en el sistema");

                entity.Property(e => e.PlantillaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Contenido).HasComment("Cuerpo de la plantilla");

                entity.Property(e => e.EncabezadoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.MargenAbajo).HasComment("Cantidad de pixeles de la margen de abajo");

                entity.Property(e => e.MargenArriba).HasComment("Cantidad de pixeles de la margen de arriba");

                entity.Property(e => e.MargenDerecha).HasComment("Cantidad de pixeles de la margen derecha");

                entity.Property(e => e.MargenIzquierda).HasComment("Cantidad de pixeles de la margen izquierda");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Nombre de la plantilla");

                entity.Property(e => e.PieDePaginaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoPlantillaId).HasComment("Llave foranea a la tabla en mención");

                entity.HasOne(d => d.Encabezado)
                    .WithMany(p => p.InverseEncabezado)
                    .HasForeignKey(d => d.EncabezadoId)
                    .HasConstraintName("fk_EncabezadoId_Plantilla");

                entity.HasOne(d => d.PieDePagina)
                    .WithMany(p => p.InversePieDePagina)
                    .HasForeignKey(d => d.PieDePaginaId)
                    .HasConstraintName("fk_PiePagina_Plantilla");
            });

            modelBuilder.Entity<PlazoContratacion>(entity =>
            {
                entity.HasComment("Almacena los plazos de contratación");

                entity.Property(e => e.PlazoContratacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.PlazoDias).HasComment("Plazo en días");

                entity.Property(e => e.PlazoMeses).HasComment("Plazo en meses");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<PolizaGarantia>(entity =>
            {
                entity.HasComment("Almacena las garantias de la póliza");

                entity.Property(e => e.PolizaGarantiaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsIncluidaPoliza).HasComment("¿Está incluida en la póliza presentada?  0. No 1. Si");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoGarantiaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAmparo)
                    .HasColumnType("numeric(18, 0)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.Vigencia)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la vigencia de la póliza");

                entity.Property(e => e.VigenciaAmparo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la vigencia de amparo");

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaGarantia)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolizaGarantia_ContratoPoliza");
            });

            modelBuilder.Entity<PolizaGarantiaActualizacion>(entity =>
            {
                entity.HasComment("Almacena la actualización de las garantias de una póliza");

                entity.Property(e => e.PolizaGarantiaActualizacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsIncluidaPoliza).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TipoGarantiaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaGarantiaActualizacion)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PolizaGar__Contr__59662CFA");
            });

            modelBuilder.Entity<PolizaListaChequeo>(entity =>
            {
                entity.HasComment("Almacena las listas de chequeo de una póliza");

                entity.Property(e => e.PolizaListaChequeoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CumpleDatosAseguradoBeneficiario).HasComment("Indica si cumple datos del beneficiario asegurado");

                entity.Property(e => e.CumpleDatosBeneficiarioGarantiaBancaria).HasComment("Indica si cumple datos del beneficiario de garantia bancaria");

                entity.Property(e => e.CumpleDatosTomadorAfianzado).HasComment("Indica si cumple datos del tomador afianzado");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.TieneCondicionesGeneralesPoliza).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneReciboPagoDatosRequeridos).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaListaChequeo)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPolizaListaChequeo_ContratoPoliza");
            });

            modelBuilder.Entity<PolizaObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones de una póliza");

                entity.Property(e => e.PolizaObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratoPolizaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoRevisionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRevision)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de revisión");

                entity.Property(e => e.Observacion).HasComment("Observación de la poliza");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.ResponsableAprobacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(400)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(400)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaObservacion)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolizaObservacion_ContratoPoliza");

                entity.HasOne(d => d.ResponsableAprobacion)
                    .WithMany(p => p.PolizaObservacion)
                    .HasForeignKey(d => d.ResponsableAprobacionId)
                    .HasConstraintName("FK_PolizaObservacion_ResponsableAprobacion");
            });

            modelBuilder.Entity<Predio>(entity =>
            {
                entity.HasComment("Almacena los predios");

                entity.Property(e => e.PredioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.CedulaCatastral)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Cédula Catastral del predio");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Dirección del predio");

                entity.Property(e => e.DocumentoAcreditacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.InstitucionEducativaSedeId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número del documento de acreditación");

                entity.Property(e => e.TipoPredioCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UbicacionLatitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Ubicación del predio coordenada Latitud");

                entity.Property(e => e.UbicacionLongitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Ubicación del predio coordenada Longitud");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.InstitucionEducativaSede)
                    .WithMany(p => p.Predio)
                    .HasForeignKey(d => d.InstitucionEducativaSedeId)
                    .HasConstraintName("FK_Predio_InstitucionEducativaSede");
            });

            modelBuilder.Entity<ProcesoSeleccion>(entity =>
            {
                entity.HasComment("Almacena los procesos de selección");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AlcanceParticular)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Descripción del Alcance");

                entity.Property(e => e.CantGrupos).HasComment("Cantidad de grupos");

                entity.Property(e => e.CantidadCotizaciones).HasComment("Cantidad de cotizaciones recibidas");

                entity.Property(e => e.CantidadProponentes).HasComment("Indica la cantidad de proponentes según el tipo de proceso de selección . Para el caso de selección privada es solo 1.  ");

                entity.Property(e => e.CantidadProponentesInvitados).HasComment("Cantidad de proponentes invitados");

                entity.Property(e => e.CondicionesAsignacionPuntaje).HasComment("Condiciones de asignación de puntaje");

                entity.Property(e => e.CondicionesFinancierasHabilitantes).HasComment("Condiciones financieras habilitantes");

                entity.Property(e => e.CondicionesJuridicasHabilitantes).HasComment("Condiciones jurídicas habilitantes");

                entity.Property(e => e.CondicionesTecnicasHabilitantes).HasComment("Condiciones técnicas habilitantes");

                entity.Property(e => e.CriteriosSeleccion)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Criterio de Selección");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("Indica si el proceso de selección esta Completo. 0. Incompleto, 1. Completo");

                entity.Property(e => e.EsDistribucionGrupos).HasComment("¿Este proceso de selección se realiza distribución del territorio en grupos? 0. No  1. Si");

                entity.Property(e => e.EstadoProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EtapaProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EvaluacionDescripcion).HasComment("Descripción de la Evaluación en Procesos de seleccion de tipo Invitaciones Cerradas y Abiertas.  ");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la Justificación");

                entity.Property(e => e.NumeroProceso)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de proceso. El número se creará de manera automática y estará compuesto por las siglas (SP, SC y SA – un consecutivo automático – el año de apertura del proceso)");

                entity.Property(e => e.Objeto)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Descripción del Objeto");

                entity.Property(e => e.ResponsableEstructuradorUsuarioid).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ResponsableTecnicoUsuarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoAlcanceCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoIntervencionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoOrdenEligibilidadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlSoporteEvaluacion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("dirección web");

                entity.Property(e => e.UrlSoporteProponentesSeleccionados)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<ProcesoSeleccionCotizacion>(entity =>
            {
                entity.HasComment("Almacena las cotizaciones de los procesos de selección");

                entity.Property(e => e.ProcesoSeleccionCotizacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Descripcion).HasComment("Descripción de la cotización");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NombreOrganizacion)
                    .HasMaxLength(1500)
                    .HasComment("Nombre de la organización");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Dirección web");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorCotizacion)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionCotizacion)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionCotizacion_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionCronograma>(entity =>
            {
                entity.HasComment("Almacena los cronogramas de los procesos de selección");

                entity.Property(e => e.ProcesoSeleccionCronogramaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("descripción de la Actividad");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EtapaActualProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaMaxima)
                    .HasColumnType("datetime")
                    .HasComment("Fecha máxima de cronograma");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroActividad).HasComment("Numero de la actividad en el cronograma");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionCronograma)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionCronograma_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionCronogramaMonitoreo>(entity =>
            {
                entity.HasComment("Almacena el monitoreo sobre el cronograma de los procesos de selección");

                entity.Property(e => e.ProcesoSeleccionCronogramaMonitoreoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasComment("Descripción del registro");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EtapaActualProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaMaxima)
                    .HasColumnType("datetime")
                    .HasComment("fecha máxima de monitoreo de cronograma");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroActividad).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ProcesoSeleccionCronogramaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProcesoSeleccionMonitoreoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccionMonitoreo)
                    .WithMany(p => p.ProcesoSeleccionCronogramaMonitoreo)
                    .HasForeignKey(d => d.ProcesoSeleccionMonitoreoId)
                    .HasConstraintName("fk_ProcesoSeleccionCronogramaMonitoreo_ProcesoSeleccionMonitoreo");
            });

            modelBuilder.Entity<ProcesoSeleccionGrupo>(entity =>
            {
                entity.HasComment("Almacena los grupos del proceso de selección");

                entity.Property(e => e.ProcesoSeleccionGrupoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NombreGrupo)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasComment("Nombre del grupo");

                entity.Property(e => e.PlazoMeses).HasComment("plazo en meses");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoPresupuestoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorMaximoCategoria)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Valor maximo de categoría de ejecución");

                entity.Property(e => e.ValorMinimoCategoria)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Valor minimo de categoría de ejecución  ");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionGrupo)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionGrupo_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionIntegrante>(entity =>
            {
                entity.HasComment("Almacena los integrantes del proceso de selección");

                entity.Property(e => e.ProcesoSeleccionIntegranteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NombreIntegrante)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Nombre del integrante");

                entity.Property(e => e.PorcentajeParticipacion).HasComment("% de Participación");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionIntegrante)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionIntegrante_ProcesoSeleccionId");

                entity.HasOne(d => d.ProcesoSeleccionProponente)
                    .WithMany(p => p.ProcesoSeleccionIntegrante)
                    .HasForeignKey(d => d.ProcesoSeleccionProponenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionIntegrante_ProcesoSeleccionProponenteId");
            });

            modelBuilder.Entity<ProcesoSeleccionMonitoreo>(entity =>
            {
                entity.HasComment("Almacena el proceso de monitoreo de los procesos de selección");

                entity.Property(e => e.ProcesoSeleccionMonitoreoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EnviadoComiteTecnico).HasComment("Indica que fue enviado al comité técnico");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroProceso)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionMonitoreo)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoMonitoreo_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones del proceso de selección");

                entity.Property(e => e.ProcesoSeleccionObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Observación asociada al Proceso");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionObservacion)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionObservacion_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionProponente>(entity =>
            {
                entity.HasComment("Almacena los proponente de un proceso de selección");

                entity.Property(e => e.ProcesoSeleccionProponenteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CedulaRepresentanteLegal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Cédula del representante legal");

                entity.Property(e => e.DireccionProponente)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Dirección del proponente");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EmailProponente).HasComment("dirección de correo electronico del proponente");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Identificador del municipio ");

                entity.Property(e => e.NombreProponente)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Nombre del proponente como Natural, juridico o Unión Temporal o Consorcio");

                entity.Property(e => e.NombreRepresentanteLegal)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Nombre del representante legal");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de identificación del proponente");

                entity.Property(e => e.ProcesoSeleccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TelefonoProponente)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Teléfono del proponente");

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoProponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionProponente)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionProponente_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesosContractualesObservacion>(entity =>
            {
                entity.Property(e => e.Archivado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Programacion>(entity =>
            {
                entity.HasComment("Almacena la programación de un contrato de construcción");

                entity.Property(e => e.ProgramacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Descripción de actividad");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Duracion).HasComment("Fecha Fin de la actividad");

                entity.Property(e => e.EsRutaCritica).HasComment("Identifica si la actividad es ruta critica: El usuario deberá indicar con un número “1” si la actividad, marcada con tipo “I”, pertenece a la ruta crítica del proceso, validando que no se ubique el número uno en un tipo de actividad C o SC.");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de la actividad");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasComment("Fecha inicio de la actividad");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.Programacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Programacion_ContratoConstruccion");
            });

            modelBuilder.Entity<ProgramacionPersonalContrato>(entity =>
            {
                entity.HasComment("Almacena la cantidad de personas programadas para un número N de semanas");

                entity.Property(e => e.ProgramacionPersonalContratoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadPersonal).HasComment("Cantidad de personal");

                entity.Property(e => e.ContratoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroSemana).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ProyectoId).HasComment("Identificador del proyecto");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ProgramacionPersonalContrato)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROGRAMACIONPERSONALCONTRATO_CONTRATO");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProgramacionPersonalContrato)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROGRAMACIONPERSONALCONTRATO_PROYECTO");
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasComment("Almacena los proyectos");

                entity.HasIndex(e => e.LlaveMen)
                    .HasName("uk_llavemen")
                    .IsUnique();

                entity.Property(e => e.ProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantPrediosPostulados).HasComment("Número de predios postulados");

                entity.Property(e => e.ConvocatoriaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CoordinacionResponsableCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EnConvocatoria).HasComment("Indica que el proyecto hace parte de una convocatoria (1. Si, 0. No)");

                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoProgramacionCodigo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoProyectoCodigoOld)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoProyectoInterventoriaCodigo)
                    .HasMaxLength(100)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoProyectoObraCodigo)
                    .HasMaxLength(100)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSesionJunta)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la sesión de junta");

                entity.Property(e => e.InstitucionEducativaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Llave Men");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Municipio donde se ejecutara el proyecto");

                entity.Property(e => e.NumeroActaJunta).HasComment("Número del acta en el que se aprueba la incorporación del proyecto.");

                entity.Property(e => e.PlazoDiasInterventoria).HasComment("Plazo en dias de la interventoria");

                entity.Property(e => e.PlazoDiasObra).HasComment("Plazo en días de la obra");

                entity.Property(e => e.PlazoMesesInterventoria).HasComment("Plazo en meses de la interventoria");

                entity.Property(e => e.PlazoMesesObra).HasComment("Plazo en meses de la obra");

                entity.Property(e => e.PredioPrincipalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.SedeId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneEstadoFase1Diagnostico).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneEstadoFase1EyD).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoIntervencionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoPredioCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlMonitoreo)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("URL para monitoreo en línea");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorInterventoria)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorObra)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorTotal)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.InstitucionEducativa)
                    .WithMany(p => p.ProyectoInstitucionEducativa)
                    .HasForeignKey(d => d.InstitucionEducativaId)
                    .HasConstraintName("FK_Proyecto_InstitucionEducativaSede");

                entity.HasOne(d => d.LocalizacionIdMunicipioNavigation)
                    .WithMany(p => p.Proyecto)
                    .HasForeignKey(d => d.LocalizacionIdMunicipio)
                    .HasConstraintName("FK_Proyecto_Localizacion");

                entity.HasOne(d => d.PredioPrincipal)
                    .WithMany(p => p.Proyecto)
                    .HasForeignKey(d => d.PredioPrincipalId)
                    .HasConstraintName("FK_Proyecto_Predio");

                entity.HasOne(d => d.Sede)
                    .WithMany(p => p.ProyectoSede)
                    .HasForeignKey(d => d.SedeId)
                    .HasConstraintName("FK_Proyecto_InstitucionEducativaSede1");
            });

            modelBuilder.Entity<ProyectoAdministrativo>(entity =>
            {
                entity.HasComment("Almacena los proyectos administrativos");

                entity.Property(e => e.ProyectoAdministrativoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Enviado).HasComment("Indica que fue enviado el proyecto administrativo");

                entity.Property(e => e.FechaCreado)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del proyecto administrativo");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<ProyectoAdministrativoAportante>(entity =>
            {
                entity.HasComment("Almacena la relación del aportante de proyectos administrativos");

                entity.Property(e => e.ProyectoAdministrativoAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEdicion)
                    .HasColumnType("datetime")
                    .HasComment("fecha en que se hace la edición");

                entity.Property(e => e.ProyectoAdminstrativoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioEdicion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que raliza la edición");

                entity.HasOne(d => d.ProyectoAdminstrativo)
                    .WithMany(p => p.ProyectoAdministrativoAportante)
                    .HasForeignKey(d => d.ProyectoAdminstrativoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoAportante_ProtectoAportanteId");
            });

            modelBuilder.Entity<ProyectoAportante>(entity =>
            {
                entity.HasComment("Almacena la relación del aportante con el proyecto");

                entity.Property(e => e.ProyectoAportanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CofinanciacionDocumentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorInterventoria)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorObra)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorTotalAportante)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.ProyectoAportante)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoAportante_Aportante");

                entity.HasOne(d => d.CofinanciacionDocumento)
                    .WithMany(p => p.ProyectoAportante)
                    .HasForeignKey(d => d.CofinanciacionDocumentoId)
                    .HasConstraintName("FK_CofinanciacionDocumento_CofinanciacionDocumentoId");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoAportante)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoAportante_Proyecto");
            });

            modelBuilder.Entity<ProyectoEntregaEtc>(entity =>
            {
                entity.ToTable("ProyectoEntregaETC");

                entity.HasComment("Almacena la entrega de los proyectos a ETC");

                entity.HasIndex(e => e.InformeFinalId)
                    .HasName("UK_informe_final_proyecto_etc")
                    .IsUnique();

                entity.Property(e => e.ProyectoEntregaEtcid)
                    .HasColumnName("ProyectoEntregaETCId")
                    .HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActaBienesServicios).HasComment("Acta de bienes y servicios");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEntregaDocumentosEtc)
                    .HasColumnName("FechaEntregaDocumentosETC")
                    .HasColumnType("datetime")
                    .HasComment("Fecha de entrega de documentación a la ETC");

                entity.Property(e => e.FechaFirmaActaBienesServicios)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de bienes y servicios de la ETC");

                entity.Property(e => e.FechaFirmaActaEngregaFisica)
                    .HasColumnType("datetime")
                    .HasComment("Fecha firma del acta de la entrega fisica de la ETC");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRecorridoObra)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de recorrido de obra a la entrega de ETC");

                entity.Property(e => e.InformeFinalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NumRadicadoDocumentosEntregaEtc)
                    .HasColumnName("NumRadicadoDocumentosEntregaETC")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número de radicado de entrega de documentos a la ETC");

                entity.Property(e => e.NumRepresentantesRecorrido).HasComment("Número de representantes que realizarán recorrido");

                entity.Property(e => e.RegistroCompletoActaBienesServicios).HasComment("Registro completo del acta de bienes y servicios");

                entity.Property(e => e.RegistroCompletoRecorridoObra).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoRemision).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.UrlActaEntregaFisica).HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.InformeFinal)
                    .WithOne(p => p.ProyectoEntregaEtc)
                    .HasForeignKey<ProyectoEntregaEtc>(d => d.InformeFinalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_informe_final_proyecto_etc");
            });

            modelBuilder.Entity<ProyectoFuentes>(entity =>
            {
                entity.HasKey(e => e.ProyectoFuenteId)
                    .HasName("PK_ProyectoAportante_copy1");

                entity.HasComment("Almacena la relación entre una fuente y un proyecto");

                entity.Property(e => e.ProyectoFuenteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Fuente)
                    .WithMany(p => p.ProyectoFuentes)
                    .HasForeignKey(d => d.FuenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProyectoA__Aport__3BB5CE82");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoFuentes)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProyectoA__Proye__3D9E16F4");
            });

            modelBuilder.Entity<ProyectoMonitoreoWeb>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoMonitoreoWeb)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoMonitoreoWeb_Proyecto");
            });

            modelBuilder.Entity<ProyectoPredio>(entity =>
            {
                entity.HasComment("Almacena los predios relacionados al proyecto");

                entity.Property(e => e.ProyectoPredioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.PredioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.Predio)
                    .WithMany(p => p.ProyectoPredio)
                    .HasForeignKey(d => d.PredioId)
                    .HasConstraintName("FK_ProyectoPredio_Predio");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoPredio)
                    .HasForeignKey(d => d.ProyectoId)
                    .HasConstraintName("FK_ProyectoPredio_Proyecto");
            });

            modelBuilder.Entity<ProyectoRequisitoTecnico>(entity =>
            {
                entity.HasComment("Almacena los requisitos técnicos de un proyectos");

                entity.Property(e => e.ProyectoRequisitoTecnicoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadAprobadas).HasComment("Cantidad de  hojas de vida  aprobadas para  cada perfil");

                entity.Property(e => e.CantidadHojasDeVida).HasComment("Cantidad de  hojas de vida  requeridas para  cada perfil");

                entity.Property(e => e.CantidadRecibidas).HasComment("Cantidad de  hojas de vida  recibidas para  cada perfil");

                entity.Property(e => e.DireccionUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("ubicación de las hojas de vida entregadas por el constructor y el Acta de aprobación de personal.");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Estado).HasComment("Completo, cuando el usuario ha registrado todos los  perfiles requeridos, según lo indicado en la pregunta  ¿Cuántos perfiles diferentes se requieren en el proyecto? o  Incompleto, cuando falta información por registrar.");

                entity.Property(e => e.EstadoHojasDeVidaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoRequisitoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaAprobacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de aprobación");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Observación");

                entity.Property(e => e.PerfilCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoRequisitoTecnico)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoRequisitoTecnico_Proyecto");
            });

            modelBuilder.Entity<RegistroPresupuestal>(entity =>
            {
                entity.HasComment("Almacena los registros presupuestales relacionados a un aportante");

                entity.HasIndex(e => new { e.AportanteId, e.NumeroRp, e.FechaRp, e.CofinanciacionDocumentoId })
                    .HasName("UK_RP")
                    .IsUnique();

                entity.Property(e => e.RegistroPresupuestalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AportanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CofinanciacionDocumentoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaRp)
                    .HasColumnName("FechaRP")
                    .HasColumnType("datetime")
                    .HasComment("Fecha de registro presupuestal");

                entity.Property(e => e.NumeroRp)
                    .HasColumnName("NumeroRP")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de RP");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorRp)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.RegistroPresupuestal)
                    .HasForeignKey(d => d.AportanteId)
                    .HasConstraintName("FK_RegistroPresupuestal_Aportante");

                entity.HasOne(d => d.CofinanciacionDocumento)
                    .WithMany(p => p.RegistroPresupuestal)
                    .HasForeignKey(d => d.CofinanciacionDocumentoId)
                    .HasConstraintName("FK_RegistroPresupuestalDocumento");
            });

            modelBuilder.Entity<RendimientosIncorporados>(entity =>
            {
                entity.HasComment("Almacena los rendimientos asociados a un cargue de pagos de rendimientos");

                entity.Property(e => e.RendimientosIncorporadosId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Aprobado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que esta aprobado si tiene valor = 1");

                entity.Property(e => e.CarguePagosRendimientosId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Consistente).HasComment("Indica si es consistente");

                entity.Property(e => e.CuentaBancaria)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de la cuenta bancaria");

                entity.Property(e => e.FechaRendimientos)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de los rendimientos incorporados");

                entity.Property(e => e.Incorporados)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor del rendimiento incorporado");

                entity.Property(e => e.ProvisionGravamenFinanciero)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor de la provisión del gravamen financiero");

                entity.Property(e => e.RendimientoIncorporar)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor del rendimiento a incorporar");

                entity.Property(e => e.Row).HasComment("Identificador de la fila");

                entity.Property(e => e.TotalGastosBancarios)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor del total de gastos bancarios");

                entity.Property(e => e.TotalGravamenFinancieroDescontado)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor del total del gravamen financiero descontado");

                entity.Property(e => e.TotalRendimientosGenerados)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Valor del total de rendimiento generados");

                entity.Property(e => e.Visitas)
                    .HasColumnType("decimal(25, 3)")
                    .HasComment("Cantidad de visitas");

                entity.HasOne(d => d.CarguePagosRendimientos)
                    .WithMany(p => p.RendimientosIncorporados)
                    .HasForeignKey(d => d.CarguePagosRendimientosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RendimientosIncorporados_CarguePagosRendimientos");
            });

            modelBuilder.Entity<RepresentanteEtcrecorrido>(entity =>
            {
                entity.HasKey(e => e.RepresentanteEtcid)
                    .HasName("PK__Represen__BDFC93A433E0C394");

                entity.ToTable("RepresentanteETCRecorrido");

                entity.HasComment("Almacena las personas representantes de una ETC");

                entity.Property(e => e.RepresentanteEtcid)
                    .HasColumnName("RepresentanteETCId")
                    .HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Cargo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Cargo en la entidad territorial");

                entity.Property(e => e.Dependencia)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre de la dependencia del representante de la ETC");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del representante que hae el recorrido de la ETC");

                entity.Property(e => e.ProyectoEntregaEtcid)
                    .HasColumnName("ProyectoEntregaETCId")
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProyectoEntregaEtc)
                    .WithMany(p => p.RepresentanteEtcrecorrido)
                    .HasForeignKey(d => d.ProyectoEntregaEtcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_proyecto_etc_representante");
            });

            modelBuilder.Entity<RequisitoTecnicoRadicado>(entity =>
            {
                entity.HasKey(e => e.RequisitoTecnicoRadicado1);

                entity.HasComment("Almacena los radicados de los requisitos técnicos");

                entity.Property(e => e.RequisitoTecnicoRadicado1)
                    .HasColumnName("RequisitoTecnicoRadicado")
                    .HasComment("Identificador del Radicado FFIE en en requisito tpecnico");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.NumeroRadicadoFfie)
                    .IsRequired()
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de aprobación de Hojas de vida");

                entity.Property(e => e.ProyectoRequisitoTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.ProyectoRequisitoTecnico)
                    .WithMany(p => p.RequisitoTecnicoRadicado)
                    .HasForeignKey(d => d.ProyectoRequisitoTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisitoTecnicoRadicado_ProyectoRequisitoTecnico");
            });

            modelBuilder.Entity<SeguimientoActuacionDerivada>(entity =>
            {
                entity.HasComment("Almacena el seguimiento de las controversias de actuación");

                entity.Property(e => e.SeguimientoActuacionDerivadaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ControversiaActuacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DescripciondeActuacionAdelantada)
                    .HasMaxLength(1500)
                    .IsUnicode(false)
                    .HasComment("Descripción de la actuación adelantada");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCompleto).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsRequiereFiduciaria).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoActuacionDerivadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaActuacionDerivada)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de la actuación derivada");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroActuacionDerivada)
                    .HasMaxLength(100)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del soporte");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.SeguimientoActuacionDerivada)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoActuacionDerivada_ControversiaActuacion");
            });

            modelBuilder.Entity<SeguimientoDiario>(entity =>
            {
                entity.HasComment("Almacena los seguimientos diarios de una contratación de un proyecto");

                entity.Property(e => e.SeguimientoDiarioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadPersonalProgramado).HasComment("Cantidad de personal programado");

                entity.Property(e => e.CantidadPersonalTrabajando).HasComment("Cantidad de personal trabajando");

                entity.Property(e => e.CausaIndisponibilidadEquipoCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CausaIndisponibilidadMaterialCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CausaIndisponibilidadProductividadCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DisponibilidadEquipoCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.DisponibilidadEquipoObservaciones).HasComment("Observaciones de la disponibilidad del equipo");

                entity.Property(e => e.DisponibilidadMaterialCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.DisponibilidadMaterialObservaciones).HasComment("Observaciones de la disponibilidad del material");

                entity.Property(e => e.DisponibilidadPersonal).HasComment("Indica si hay disponibilidad de personal");

                entity.Property(e => e.DisponibilidadPersonalObservaciones).HasComment("Observaciones de la disponibilidad de personal");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSeguimiento)
                    .HasColumnType("date")
                    .HasComment("Fecha de sguimiento ");

                entity.Property(e => e.FechaValidacion)
                    .HasColumnType("datetime")
                    .HasComment("fecha de validación");

                entity.Property(e => e.FechaVerificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de verificación");

                entity.Property(e => e.NumeroHorasRetrasoEquipo).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroHorasRetrasoMaterial).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroHorasRetrasoPersonal).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.NumeroHorasRetrasoProductividad).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ProductividadCodigo)
                    .HasMaxLength(2)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ProductividadObservaciones).HasComment("Observaciones de productividad");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoValidacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificacion).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeGeneroRetrasoEquipo).HasComment("Indica si se genero retrazo del equipo");

                entity.Property(e => e.SeGeneroRetrasoMaterial).HasComment("Indica si se genero retrazo del material");

                entity.Property(e => e.SeGeneroRetrasoPersonal).HasComment("Indica si se genero retrazo del personal");

                entity.Property(e => e.SeGeneroRetrasoProductividad).HasComment("Indica si se genero retrazo de la productividad");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SeguimientoDiario)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoDiario_ContratacionProyecto");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoDiario)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoDiario_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoDiarioObservaciones>(entity =>
            {
                entity.HasComment("Almacena las observaciones de un seguimiento diario");

                entity.Property(e => e.SeguimientoDiarioObservacionesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivado).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsSupervision).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.SeguimientoDiarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SeguimientoDiario)
                    .WithMany(p => p.SeguimientoDiarioObservaciones)
                    .HasForeignKey(d => d.SeguimientoDiarioId)
                    .HasConstraintName("FK_SeguimientoDiarioObservaciones_SeguimientoDiario");
            });

            modelBuilder.Entity<SeguimientoSemanal>(entity =>
            {
                entity.HasComment("Almacena los seguimientos semanales de una contratación de un proyecto");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoMuestrasCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de la actividad");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasComment("Fecha inicio de la actividad");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaModificacionAvalar)
                    .HasColumnType("datetime")
                    .HasComment("fecha de modificación para avalar");

                entity.Property(e => e.FechaModificacionVerificar)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de modificación para verificar");

                entity.Property(e => e.FechaRegistroCompletoApoyo)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del registro completo por parte del apoyo a la supervisión");

                entity.Property(e => e.FechaRegistroCompletoInterventor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de registro completo del interventor");

                entity.Property(e => e.FechaRegistroCompletoSupervisor)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del registro completo por parte del supervisor");

                entity.Property(e => e.NumeroSemana).HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoAvalar).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoMuestras).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoVerificar).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SeguimientoSemanal)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanal_ContratacionProyecto");
            });

            modelBuilder.Entity<SeguimientoSemanalAvanceFinanciero>(entity =>
            {
                entity.HasComment("Almacena el avance financieron del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalAvanceFinancieroId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.GenerarAlerta).HasComment("Indica que se genera alerta por incumplimiento del avance financiero");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereObservacion).HasComment("Indica si requiere observación");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalAvanceFinancieroObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFinanciero_SeguimientoSemanalApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalAvanceFinancieroObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFinanciero_SeguimientoSemanalSupervisor");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalAvanceFinanciero)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFinanciero_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalAvanceFisico>(entity =>
            {
                entity.HasComment("Almacena el avance físico del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalAvanceFisicoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AvanceFisicoSemanal)
                    .HasColumnType("decimal(18, 3)")
                    .HasComment("Valorsegún unidad de medida");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoObraCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ProgramacionSemanal)
                    .HasColumnType("decimal(18, 3)")
                    .HasComment("Cuantia de la programación semanal");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalAvanceFisicoObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFisico_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalAvanceFisicoObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFisico_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalAvanceFisico)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFisico_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalAvanceFisicoProgramacion>(entity =>
            {
                entity.HasComment("Almacena la relación de la programación respecto al avance físico del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalAvanceFisicoProgramacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AvanceFisicoCapitulo)
                    .HasColumnType("decimal(38, 3)")
                    .HasComment("División jerarquica del avance físico");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ProgramacionCapitulo).HasColumnType("decimal(3, 0)");

                entity.Property(e => e.ProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SeguimientoSemanalAvanceFisicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Programacion)
                    .WithMany(p => p.SeguimientoSemanalAvanceFisicoProgramacion)
                    .HasForeignKey(d => d.ProgramacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName(" FK_SeguimientoSemanalAvanceFisicoProgramacion_Programacion");

                entity.HasOne(d => d.SeguimientoSemanalAvanceFisico)
                    .WithMany(p => p.SeguimientoSemanalAvanceFisicoProgramacion)
                    .HasForeignKey(d => d.SeguimientoSemanalAvanceFisicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalAvanceFisicoProgramacion_SeguimientoSemanalAvanceFisico");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObra>(entity =>
            {
                entity.HasComment("Almacena la gestión de obra del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalGestionObra)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObra_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraAlerta>(entity =>
            {
                entity.HasComment("Almacena las alertas relacionadas con la gestión de obra del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraAlertaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Alerta).HasComment("Descripción de la alerta");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeIdentificaronAlertas).HasComment("Indica si se identificaron alertas");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAlertaObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraAlerta_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAlertaObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraAlerta_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAlerta)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraAlerta_SeguimientoSemanalGestionObra");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraAmbiental>(entity =>
            {
                entity.HasComment("Almacena la gestión de obra ambiental del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraAmbientalId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ManejoMaterialesInsumoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ManejoOtroId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ManejoResiduosConstruccionDemolicionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ManejoResiduosPeligrososEspecialesId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeEjecutoGestionAmbiental).HasComment("Indica si se ejecuto la gestión ambiental");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneManejoMaterialesInsumo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneManejoOtro).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneManejoResiduosConstruccionDemolicion).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneManejoResiduosPeligrososEspeciales).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ManejoMaterialesInsumo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbiental)
                    .HasForeignKey(d => d.ManejoMaterialesInsumoId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraAmbiental_ManejoMaterialesInsumosId_1");

                entity.HasOne(d => d.ManejoOtro)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbiental)
                    .HasForeignKey(d => d.ManejoOtroId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraAmbiental_ManejoOtro_1");

                entity.HasOne(d => d.ManejoResiduosConstruccionDemolicion)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbiental)
                    .HasForeignKey(d => d.ManejoResiduosConstruccionDemolicionId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraAmbiental_ManejoResiduosConstruccionDemolicion_1");

                entity.HasOne(d => d.ManejoResiduosPeligrososEspeciales)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbiental)
                    .HasForeignKey(d => d.ManejoResiduosPeligrososEspecialesId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraAmbiental_ManejoResiduosPeligrososEspeciales_1");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbientalObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_SeguimientoSemanalAvanceGestionObraAmbiental_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbientalObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_SeguimientoSemanalAvanceGestionObraAmbiental_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.SeguimientoSemanalGestionObraAmbiental)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraAmbiental_SeguimientoSemanalGestionObra_1");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraCalidad>(entity =>
            {
                entity.HasComment("Almacena la gestión de obra calidad del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraCalidadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeRealizaronEnsayosLaboratorio).HasComment("Indica si se realizaron ensayos de laboratorio");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraCalidadObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraCalidad_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalGestionObraCalidadObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraCalidad_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.SeguimientoSemanalGestionObraCalidad)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObraCalidad_SeguimientoSemanalGestionObra");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraSeguridadSalud>(entity =>
            {
                entity.HasComment("Almacena la gestión de obrade seguridad y salud del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraSeguridadSaludId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadAccidentes).HasComment("Cantidad de accidentes");

                entity.Property(e => e.CumpleRevisionElementosProyeccion).HasComment("Indica si cumple revisión de elementos de protección");

                entity.Property(e => e.CumpleRevisionSenalizacion).HasComment("Indica si cumple revisión de señalización");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionCapacitacion).HasComment("Observaciones de la capacitación");

                entity.Property(e => e.ObservacionRevisionElementosProteccion).HasComment("Observaciones en la revisión de elementos de protección");

                entity.Property(e => e.ObservacionRevisionSenalizacion).HasComment("Observaciones en la revisión de la señalización");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeRealizoCapacitacion).HasComment("Indica si se realizo capacitación");

                entity.Property(e => e.SeRealizoRevisionElementosProteccion).HasComment("Indica si se realizo revisión de elementos de protección");

                entity.Property(e => e.SeRealizoRevisionSenalizacion).HasComment("Indica si se realizo revisión de señalización");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TemaCapacitacion)
                    .HasMaxLength(300)
                    .HasComment("Descripción del tema de capacitación");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteGestion)
                    .HasMaxLength(255)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSeguridadSaludObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSeguridadSalud_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSeguridadSaludObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSeguridadSalud_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSeguridadSalud)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSeguridadSalud_SeguimientoSemanalGestionObra_1");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraSocial>(entity =>
            {
                entity.HasComment("Almacena la gestión de obra social del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalGestionObraSocialId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadEmpleosDirectos).HasComment("Cantidad de empleos directos");

                entity.Property(e => e.CantidadEmpleosIndirectos).HasComment("Cantidad de empleos indirectos");

                entity.Property(e => e.CantidadTotalEmpleos).HasComment("Cantidad de total de empleos");

                entity.Property(e => e.Conclusion).HasComment("Conclusión del tema de la comunidad");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionRealizaronReuniones).HasComment("Observaciones si realizaron reuniones");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeRealizaronReuniones).HasComment("Indica si se realizaron reuniones");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TemaComunidad).HasComment("Descripción del tema de comunidad");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteGestion)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSocialObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSocial_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSocialObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSocial_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.SeguimientoSemanalGestionObraSocial)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalGestionObraSocial_SeguimientoSemanalGestionObra_1");
            });

            modelBuilder.Entity<SeguimientoSemanalObservacion>(entity =>
            {
                entity.HasComment("Almacena las observaciones sdel seguimiento semanal de obra");

                entity.Property(e => e.SeguimientoSemanalObservacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Archivada).HasComment("Indica si la observación esta archivada");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsSupervisor).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionPadreId).HasComment("Llave foranea a la misma tabla");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacion).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalObservacion)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalObervavion_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalPersonalObra>(entity =>
            {
                entity.HasComment("Almacena el personal de obra de seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalPersonalObraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantidadPersonal).HasComment("Cantidad de personal");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalPersonalObra)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalPersonalObra_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalRegistrarComiteObra>(entity =>
            {
                entity.HasComment("Almacena los comités de obra del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalRegistrarComiteObraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaComite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha del comité de obra");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.NumeroComite)
                    .HasMaxLength(255)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteComite)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalRegistrarComiteObraObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_SeguimientoSemanalRegistratComiteObra_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalRegistrarComiteObraObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_SeguimientoSemanalRegistratComiteObra_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalRegistrarComiteObra)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalRegistratComiteObra_SeguimientoSemanal_1");
            });

            modelBuilder.Entity<SeguimientoSemanalRegistroFotografico>(entity =>
            {
                entity.HasComment("Almacena el registro fotografico del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalRegistroFotograficoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Descripcion).HasComment("Descripción del registro");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorId).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyo).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisor).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisor).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UrlSoporteFotografico)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyo)
                    .WithMany(p => p.SeguimientoSemanalRegistroFotograficoObservacionApoyo)
                    .HasForeignKey(d => d.ObservacionApoyoId)
                    .HasConstraintName("fk_SeguimientoSemanalRegistroFotografico_SeguimientoSemanalObservacionApoyo");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoSemanalRegistroFotograficoObservacionSupervisor)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("fk_SeguimientoSemanalRegistroFotografico_SeguimientoSemanalObservacionSupervisor");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalRegistroFotografico)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalRegistroFotografico_SeguimientoSemanal_1");
            });

            modelBuilder.Entity<SeguimientoSemanalReporteActividad>(entity =>
            {
                entity.HasComment("Almacena el reporte de actividades del seguimiento semanal");

                entity.Property(e => e.SeguimientoSemanalReporteActividadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ActividadAdministrativaFinanciera).HasComment("Descripción de la actividad por financiera");

                entity.Property(e => e.ActividadAdministrativaFinancieraSiguiente).HasComment("Descripción de la siguiente actividad por financiera");

                entity.Property(e => e.ActividadLegal).HasComment("Descripción de la actividad por el área legal");

                entity.Property(e => e.ActividadLegalSiguiente).HasComment("Descripción de la siguiente actividad por el área legal");

                entity.Property(e => e.ActividadTecnica).HasComment("Descripción de la actividad por el área técnica");

                entity.Property(e => e.ActividadTecnicaSiguiente).HasComment("Descripción de la siguiente actividad por el área técnica");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ObservacionApoyoIdActividad).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionApoyoIdActividadSiguiente).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionApoyoIdEstadoContrato).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorIdActividad).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorIdActividadSiguiente).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.ObservacionSupervisorIdEstadoContrato).HasComment("Llave foranea a la tabla en usuario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoActividad).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoActividadSiguiente).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoEstadoContrato).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyoActividad).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyoActividadSiguiente).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionApoyoEstadoContrato).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisorActividad).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisorActividadSiguiente).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RegistroCompletoObservacionSupervisorEstadoContrato).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.ResumenEstadoContrato).HasComment("Resumen del estado del contrato");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TieneObservacionApoyoActividad).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionApoyoActividadSiguiente).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionApoyoEstadoContrato).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisorActividad).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisorActividadSiguiente).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneObservacionSupervisorEstadoContrato).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ObservacionApoyoIdActividadNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionApoyoIdActividadNavigation)
                    .HasForeignKey(d => d.ObservacionApoyoIdActividad)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionApoyo_Actividad");

                entity.HasOne(d => d.ObservacionApoyoIdActividadSiguienteNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionApoyoIdActividadSiguienteNavigation)
                    .HasForeignKey(d => d.ObservacionApoyoIdActividadSiguiente)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionApoyo_ActividadSiguiente");

                entity.HasOne(d => d.ObservacionApoyoIdEstadoContratoNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionApoyoIdEstadoContratoNavigation)
                    .HasForeignKey(d => d.ObservacionApoyoIdEstadoContrato)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionApoyo_EstadoContrato");

                entity.HasOne(d => d.ObservacionSupervisorIdActividadNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadNavigation)
                    .HasForeignKey(d => d.ObservacionSupervisorIdActividad)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionSupervisor_Actividad");

                entity.HasOne(d => d.ObservacionSupervisorIdActividadSiguienteNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadSiguienteNavigation)
                    .HasForeignKey(d => d.ObservacionSupervisorIdActividadSiguiente)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionSupervisor_ActividadSiguiente");

                entity.HasOne(d => d.ObservacionSupervisorIdEstadoContratoNavigation)
                    .WithMany(p => p.SeguimientoSemanalReporteActividadObservacionSupervisorIdEstadoContratoNavigation)
                    .HasForeignKey(d => d.ObservacionSupervisorIdEstadoContrato)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanalObservacionSupervisor_EstadoContrato");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalReporteActividad)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguimientoSemanalReporteActividad_SeguimientoSemanal_1");
            });

            modelBuilder.Entity<SeguimientoSemanalTemp>(entity =>
            {
                entity.Property(e => e.EstadoMuestrasCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoApoyo).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoInterventor).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoSupervisor).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.AjusteProgramaion)
                    .WithMany(p => p.SeguimientoSemanalTemp)
                    .HasForeignKey(d => d.AjusteProgramaionId)
                    .HasConstraintName("FK_SeguimientoSemanalTemp_AjusteProgramacion");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SeguimientoSemanalTemp)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalTemp_ContratacionProyecto");
            });

            modelBuilder.Entity<SeguridadSaludCausaAccidente>(entity =>
            {
                entity.HasKey(e => e.SeguridadSaludCausaAccidentesId)
                    .HasName("PK__Segurida__60218A2A3128448E");

                entity.HasComment("Almacena las causas del accidente relacionadas a una gestión de obra en un seguimiento semanal");

                entity.Property(e => e.SeguridadSaludCausaAccidentesId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CausaAccidenteCodigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.SeguimientoSemanalGestionObraSeguridadSaludId).HasComment("Llave foranea a la tabla SeguimientoSemanalGestionObraSeguridadSalud");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.SeguimientoSemanalGestionObraSeguridadSalud)
                    .WithMany(p => p.SeguridadSaludCausaAccidente)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraSeguridadSaludId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguridadSaludCausaAccidente_SeguimientoSemanalGestionObraSeguridadSalud_1");
            });

            modelBuilder.Entity<SesionComentario>(entity =>
            {
                entity.HasComment("Almacena los comentarios de una sesión de comité");

                entity.Property(e => e.SesionComentarioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoId).HasComment("identificador del registro de sesión de comité");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActaVoto)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Estado del voto del acta en sesión de comité");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se hace la acción");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MiembroSesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la observación");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValidacionVoto).HasComment("Indica si se valido el voto");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComentario)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComentario_ComiteTecnico");

                entity.HasOne(d => d.MiembroSesionParticipante)
                    .WithMany(p => p.SesionComentario)
                    .HasForeignKey(d => d.MiembroSesionParticipanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComentario_SesionParticipante");
            });

            modelBuilder.Entity<SesionComiteSolicitud>(entity =>
            {
                entity.HasComment("Almacena la relación de las solicitudes con las sesiones de comité");

                entity.Property(e => e.SesionComiteSolicitudId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantCompromisos).HasComment("Cantidad de Compromisos que genera");

                entity.Property(e => e.CantCompromisosFiduciario).HasComment("Cantidad de compromisos de la sesión de comité fiduciario");

                entity.Property(e => e.ComiteTecnicoFiduciarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.DesarrolloSolicitud).HasComment("Observación del desarrollo de la solicitud del comité técnico");

                entity.Property(e => e.DesarrolloSolicitudFiduciario).HasComment("Observación del desarrollo de la solicitud del comité fiduciario");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoActaCodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoActaCodigoFiduciario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaComiteFiduciario)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de comité fiduciario");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.GeneraCompromiso).HasComment("Indica que genera compromiso");

                entity.Property(e => e.GeneraCompromisoFiduciario).HasComment("Indica que se genera un compromiso en comité fiduciario");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionesFiduciario).HasComment("Observaciones del comité fiduciario");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RegistroCompletoFiduciaria).HasComment("Indica que el registro esta completo para el sufijo seguido de las palabras registro completo");

                entity.Property(e => e.RequiereVotacion).HasComment("Indica si requiere votación");

                entity.Property(e => e.RequiereVotacionFiduciario).HasComment("Indica si requiere votación en comité fiduciario");

                entity.Property(e => e.RutaSoporteVotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Ruta de url de soporte de votación.");

                entity.Property(e => e.RutaSoporteVotacionFiduciario).HasComment("Ruta del soporte de votación del comité fiduciario");

                entity.Property(e => e.SolicitudId).HasComment("Identificador de la Solicitud depenciendo del Tipo de Solicitud. DefensaJudicialId, ControversiaContractualId, NovedadContractualId, ContratacionId,ProcesoSeleccionId");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioComiteFiduciario)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario del sistema que realiza la inclusión de la solicitud en un comité fiduciario");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnicoFiduciario)
                    .WithMany(p => p.SesionComiteSolicitudComiteTecnicoFiduciario)
                    .HasForeignKey(d => d.ComiteTecnicoFiduciarioId)
                    .HasConstraintName("FK_SesionComiteSolicitud_ComiteTecnico1");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteSolicitudComiteTecnico)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteSolicitud_ComiteTecnico");
            });

            modelBuilder.Entity<SesionComiteTecnicoCompromiso>(entity =>
            {
                entity.HasComment("Almacena los compromisos de una sesión de comité técnico");

                entity.Property(e => e.SesionComiteTecnicoCompromisoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaCumplimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de cumplimiento");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Responsable)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Nombre del responsable");

                entity.Property(e => e.Tarea)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Descripción de la tarea");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteTecnicoCompromiso)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTecnicoCompromiso_ComiteTecnico");
            });

            modelBuilder.Entity<SesionComiteTema>(entity =>
            {
                entity.HasKey(e => e.SesionTemaId);

                entity.HasComment("Almacena los temas de una sesión de comité");

                entity.Property(e => e.SesionTemaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.CantCompromisos).HasComment("Cantidad de compromisos de la sesión de comité técnico");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAprobado).HasComment("Indica que el tema fue aprobado. 1. Si, 2. No");

                entity.Property(e => e.EsProposicionesVarios).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoTemaCodigo)
                    .HasMaxLength(255)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.GeneraCompromiso).HasComment("Indica que se genera un compromiso en comité técnico");

                entity.Property(e => e.Observaciones).HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionesDecision).HasComment("Observaciones de la decisión");

                entity.Property(e => e.RegistroCompleto).HasComment("Indica que el registro queda completo");

                entity.Property(e => e.RequiereVotacion).HasComment("Indica si requiere votación");

                entity.Property(e => e.ResponsableCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Ruta del archivo adjunto");

                entity.Property(e => e.Tema)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Descripción del tema");

                entity.Property(e => e.TiempoIntervencion).HasComment("tiempo de intervención dado en minutos");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteTema)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .HasConstraintName("FK_SesionComiteTema_ComiteTecnico");
            });

            modelBuilder.Entity<SesionInvitado>(entity =>
            {
                entity.HasComment("Almacena los invitados a una sesión de comité");

                entity.Property(e => e.SesionInvitadoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Cargo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Cargo del Invitado");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Entidad)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre de la Entidad");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre Invitado");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionInvitado)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .HasConstraintName("FK_SesionInvitado_ComiteTecnico");
            });

            modelBuilder.Entity<SesionParticipante>(entity =>
            {
                entity.HasComment("Almacena los participantes a una sesión de comité");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("date")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionParticipante)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionParticipante_ComiteTecnico");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.SesionParticipante)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionParticipante_Usuario");
            });

            modelBuilder.Entity<SesionParticipanteVoto>(entity =>
            {
                entity.HasComment("Almacena el voto de los participantes a una sesión de comité");

                entity.Property(e => e.SesionParticipanteVotoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EsAprobado).HasComment("Identifica si es aprobado el tema por el invitado 0. No, 1. Si");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ObservacionesDevolucion)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Observaciones cuando se quiere devolver la solicitud");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionParticipanteVoto)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionParticipanteVoto_ComiteTecnico");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.SesionParticipanteVoto)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionParticipanteVoto_SesionParticipante");
            });

            modelBuilder.Entity<SesionSolicitudCompromiso>(entity =>
            {
                entity.HasComment("Almacena los compromisos de las solicitudes de un comité");

                entity.Property(e => e.SesionSolicitudCompromisoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCumplido).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EsFiduciario)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaCumplimiento)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de cumplimiento");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.ResponsableSesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionComiteSolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Tarea)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Descripción de la tarea");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ResponsableSesionParticipante)
                    .WithMany(p => p.SesionSolicitudCompromiso)
                    .HasForeignKey(d => d.ResponsableSesionParticipanteId)
                    .HasConstraintName("FK_SesionSolicitudCompromiso_SesionParticipante");

                entity.HasOne(d => d.SesionComiteSolicitud)
                    .WithMany(p => p.SesionSolicitudCompromiso)
                    .HasForeignKey(d => d.SesionComiteSolicitudId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudCompromiso_SesionComiteSolicitud");
            });

            modelBuilder.Entity<SesionSolicitudObservacionActualizacionCronograma>(entity =>
            {
                entity.HasComment("Almacena las relaciones de los participantes del cronograma del proceso de selección");

                entity.Property(e => e.SesionSolicitudObservacionActualizacionCronogramaId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.ProcesoSeleccionCronogramaMonitoreoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionComiteSolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ProcesoSeleccionCronogramaMonitoreo)
                    .WithMany(p => p.SesionSolicitudObservacionActualizacionCronograma)
                    .HasForeignKey(d => d.ProcesoSeleccionCronogramaMonitoreoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudObservacionActualizacionCronograma_ProcesoSeleccionCronogramaMonitoreo");

                entity.HasOne(d => d.SesionComiteSolicitud)
                    .WithMany(p => p.SesionSolicitudObservacionActualizacionCronograma)
                    .HasForeignKey(d => d.SesionComiteSolicitudId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudObservacionActualizacionCronograma_SesionComiteSolicitud");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.SesionSolicitudObservacionActualizacionCronograma)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .HasConstraintName("FK_SesionSolicitudObservacionActualizacionCronograma_SesionParticipante");
            });

            modelBuilder.Entity<SesionSolicitudObservacionProyecto>(entity =>
            {
                entity.HasComment("Almacena las observaciones de una sesión de comité respecto a la contratación del proyecto");

                entity.Property(e => e.SesionSolicitudObservacionProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ContratacionProyectoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .IsUnicode(false)
                    .HasComment("Observación del proyecto según el participante");

                entity.Property(e => e.SesionComiteSolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SesionSolicitudObservacionProyecto)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudObservacionProyecto_ContratacionProyecto");

                entity.HasOne(d => d.SesionComiteSolicitud)
                    .WithMany(p => p.SesionSolicitudObservacionProyecto)
                    .HasForeignKey(d => d.SesionComiteSolicitudId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudObservacionProyecto_SesionComiteSolicitud");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.SesionSolicitudObservacionProyecto)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .HasConstraintName("FK_SesionSolicitudObservacionProyecto_SesionParticipante");
            });

            modelBuilder.Entity<SesionSolicitudVoto>(entity =>
            {
                entity.HasComment("Almacena los votos de una solicitud de un comité");

                entity.Property(e => e.SesionSolicitudVotoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ComiteTecnicoFiduciarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAprobado).HasComment("Indica si la solicitud ha sido aprobada por el participante de la sesión 0. No aprobado, 1. Aprobado  ");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .IsUnicode(false)
                    .HasComment("Observación del proyecto según el participante");

                entity.Property(e => e.SesionComiteSolicitudId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SesionComiteSolicitud)
                    .WithMany(p => p.SesionSolicitudVoto)
                    .HasForeignKey(d => d.SesionComiteSolicitudId)
                    .HasConstraintName("FK_SesionSolicitudVoto_SesionComiteSolicitud");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.SesionSolicitudVoto)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionSolicitudVoto_SesionParticipante");
            });

            modelBuilder.Entity<SesionTemaVoto>(entity =>
            {
                entity.HasComment("Almacena los votos de los participantes sobre un tema de una sesión");

                entity.Property(e => e.SesionTemaVotoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsAprobado).HasComment("Indica si la solicitud ha sido aprobada por el participante de la sesión 0. No aprobado, 1. Aprobado  ");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Observacion)
                    .IsUnicode(false)
                    .HasComment("Observación del proyecto según el participante");

                entity.Property(e => e.SesionParticipanteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SesionTemaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.SesionParticipante)
                    .WithMany(p => p.SesionTemaVoto)
                    .HasForeignKey(d => d.SesionParticipanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionTemaVoto_SesionParticipante");

                entity.HasOne(d => d.SesionTema)
                    .WithMany(p => p.SesionTemaVoto)
                    .HasForeignKey(d => d.SesionTemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionTemaVoto_SesionComiteTema");
            });

            modelBuilder.Entity<Solicitud>(entity =>
            {
                entity.HasComment("Almacena las solicitudes");

                entity.Property(e => e.SolicitudId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.EsCompleto).HasComment("0. Incompleto, 1. Completo");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaEnvioDocumentacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de envio  de la documentación");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaTramite)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se realiza el trámite");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Número de radicado en FFIE de solicitud.");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.RutaMinuta)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("ruta de la minuta ");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<SolicitudPago>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstaRechazada).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacionFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaAsignacionSacFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRadicacionSacContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaRadicacionSacFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaRadicacionSacTecnica).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompleto).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoAutorizar).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoValidacionFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoVerificacionFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaSubsanacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroRadicacionSacContratista).HasMaxLength(15);

                entity.Property(e => e.NumeroRadicacionSacFinanciera).HasMaxLength(20);

                entity.Property(e => e.NumeroRadicacionSacTecnica).HasMaxLength(20);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionRadicacionSacTecnica).HasMaxLength(2000);

                entity.Property(e => e.TieneObservacion).HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporteFinanciera).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SolicitudPago)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .HasConstraintName("FK_SolicitudPago_ContratacionProyecto");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.SolicitudPago)
                    .HasForeignKey(d => d.ContratoId)
                    .HasConstraintName("FK_SolicitudPagoId_ContratoId");

                entity.HasOne(d => d.OrdenGiro)
                    .WithMany(p => p.SolicitudPago)
                    .HasForeignKey(d => d.OrdenGiroId)
                    .HasConstraintName("FK_SolicitudPago_OrdenGiro");
            });

            modelBuilder.Entity<SolicitudPagoCargarFormaPago>(entity =>
            {
                entity.Property(e => e.FaseConstruccionFormaPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FasePreConstruccionFormaPagoCodigo).HasMaxLength(2);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoCargarFormaPago)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .HasConstraintName("FK_SolicitudPagoCargarFormaPago_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoExpensas>(entity =>
            {
                entity.Property(e => e.ConceptoPagoCriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroFactura)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoSac)
                    .HasColumnName("NumeroRadicadoSAC")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorFacturadoConcepto).HasColumnType("numeric(38, 2)");

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoExpensas)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoExpensas_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoFactura>(entity =>
            {
                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Numero)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorFacturadoConDescuento).HasColumnType("numeric(38, 2)");

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoFactura)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .HasConstraintName("FK_SolicitudPagoFaseFacturaId_SolicitudPagoId");
            });

            modelBuilder.Entity<SolicitudPagoFase>(entity =>
            {
                entity.Property(e => e.EsAnticipio).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SolicitudPagoFase)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .HasConstraintName("FK_SolicitudPagoFase_ContratacionProyectoId");

                entity.HasOne(d => d.SolicitudPagoRegistrarSolicitudPago)
                    .WithMany(p => p.SolicitudPagoFase)
                    .HasForeignKey(d => d.SolicitudPagoRegistrarSolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoFase_SolicitudPagoRegistrarSolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoFaseAmortizacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAmortizacion).HasColumnType("decimal(25, 1)");

                entity.HasOne(d => d.SolicitudPagoFase)
                    .WithMany(p => p.SolicitudPagoFaseAmortizacion)
                    .HasForeignKey(d => d.SolicitudPagoFaseId)
                    .HasConstraintName("FK_SolicitudPagoFaseAmortizacionAnticipo_SolicitudPagoFase");
            });

            modelBuilder.Entity<SolicitudPagoFaseCriterio>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoCriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");

                entity.HasOne(d => d.SolicitudPagoFase)
                    .WithMany(p => p.SolicitudPagoFaseCriterio)
                    .HasForeignKey(d => d.SolicitudPagoFaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoFase_SolicitudPagoFaseId");
            });

            modelBuilder.Entity<SolicitudPagoFaseCriterioConceptoPago>(entity =>
            {
                entity.Property(e => e.ConceptoPagoCriterio)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturadoConcepto).HasColumnType("decimal(30, 0)");

                entity.HasOne(d => d.SolicitudPagoFaseCriterio)
                    .WithMany(p => p.SolicitudPagoFaseCriterioConceptoPago)
                    .HasForeignKey(d => d.SolicitudPagoFaseCriterioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoFaseCriterioConceptoPago_SolicitudPagoFaseCriterio");
            });

            modelBuilder.Entity<SolicitudPagoFaseFacturaDescuento>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("numeric(38, 2)");

                entity.HasOne(d => d.SolicitudPagoFase)
                    .WithMany(p => p.SolicitudPagoFaseFacturaDescuento)
                    .HasForeignKey(d => d.SolicitudPagoFaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoFaseFacturaDescuentoSolicitudPagoFase");
            });

            modelBuilder.Entity<SolicitudPagoListaChequeo>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoCriterioCodigo).HasMaxLength(2);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ListaChequeo)
                    .WithMany(p => p.SolicitudPagoListaChequeo)
                    .HasForeignKey(d => d.ListaChequeoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoListaChequeo_ListaChequeo");

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoListaChequeo)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoListaChequeo_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoListaChequeoRespuesta>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).HasMaxLength(2000);

                entity.Property(e => e.RespuestaCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValidacionObservacion).HasMaxLength(2000);

                entity.Property(e => e.ValidacionRespuestaCodigo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VerificacionObservacion).HasMaxLength(2000);

                entity.Property(e => e.VerificacionRespuestaCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.ListaChequeoItem)
                    .WithMany(p => p.SolicitudPagoListaChequeoRespuesta)
                    .HasForeignKey(d => d.ListaChequeoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_SolicitudPagoListaChequeoRespuesta_ListaChequeoItem");

                entity.HasOne(d => d.SolicitudPagoListaChequeo)
                    .WithMany(p => p.SolicitudPagoListaChequeoRespuesta)
                    .HasForeignKey(d => d.SolicitudPagoListaChequeoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_SolicitudPagoListaChequeoRespuesta_SolicitudPagoListaChequeo");
            });

            modelBuilder.Entity<SolicitudPagoObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.SolicitudPagoObservacion)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_SolicitudPagoObervacion_MenuId");

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoObservacion)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoObervacion_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoOtrosCostosServicios>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroFactura)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoSac)
                    .HasColumnName("NumeroRadicadoSAC")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("decimal(25, 3)");

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoOtrosCostosServicios)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoOtrosCostosServicios_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoRegistrarSolicitudPago>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("date");

                entity.Property(e => e.NumeroRadicadoSac)
                    .HasColumnName("NumeroRadicadoSAC")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoRegistrarSolicitudPago)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SolicitudPagoContratoFormaPago_SolicitudPago");
            });

            modelBuilder.Entity<SolicitudPagoSoporteSolicitud>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SolicitudPago)
                    .WithMany(p => p.SolicitudPagoSoporteSolicitud)
                    .HasForeignKey(d => d.SolicitudPagoId)
                    .HasConstraintName("FK_SolicitudPagoSoporteSolicitud_SolicitudPago");
            });

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.DiagramId)
                    .HasColumnName("diagram_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<TemaCompromiso>(entity =>
            {
                entity.HasComment("Almacena los compromisos de un tema");

                entity.Property(e => e.TemaCompromisoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado).HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.EsCumplido).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaCumplimiento)
                    .HasColumnType("datetime")
                    .HasComment("fecha de cumplimiento");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Responsable).HasComment("Nombre del responsable");

                entity.Property(e => e.SesionTemaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Tarea)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Descripción de la tarea");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ResponsableNavigation)
                    .WithMany(p => p.TemaCompromiso)
                    .HasForeignKey(d => d.Responsable)
                    .HasConstraintName("FK_Responsable");

                entity.HasOne(d => d.SesionTema)
                    .WithMany(p => p.TemaCompromiso)
                    .HasForeignKey(d => d.SesionTemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemaCompromiso_SesionComiteTema");
            });

            modelBuilder.Entity<TemaCompromisoSeguimiento>(entity =>
            {
                entity.HasComment("Almacena el seguimiento de los compromisos de un tema");

                entity.Property(e => e.TemaCompromisoSeguimientoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.Tarea)
                    .HasMaxLength(500)
                    .HasComment("Descripción de la tarea");

                entity.Property(e => e.TemaCompromisoId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.HasOne(d => d.TemaCompromiso)
                    .WithMany(p => p.TemaCompromisoSeguimiento)
                    .HasForeignKey(d => d.TemaCompromisoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemaCompromisoId");
            });

            modelBuilder.Entity<Temp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Temp");

                entity.Property(e => e.EstadoObra).HasMaxLength(250);

                entity.Property(e => e.EstadoObraCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaUltimoReporte)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContrato)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<TempFlujoInversion>(entity =>
            {
                entity.HasComment("Tabla utilizada para el cargue de proyectos masivos");

                entity.Property(e => e.TempFlujoInversionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.AjusteProgramacionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ArchivoCargueId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EstaValidado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.MesEjecucionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Posicion).HasComment("Posición en la visualización");

                entity.Property(e => e.PosicionCapitulo).HasComment("Posición en la visualización");

                entity.Property(e => e.ProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.SeguimientoSemanalId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Semana)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Número de la semana");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.Valor)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempFlujoInversion)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempFlujo__Archi__17236851");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.TempFlujoInversion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .HasConstraintName("FK__TempFlujo__Contr__18178C8A");
            });

            modelBuilder.Entity<TempOrdenLegibilidad>(entity =>
            {
                entity.HasComment("Tabla utilizada para el cargue de proyectos masivos");

                entity.Property(e => e.TempOrdenLegibilidadId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ArchivoCargueId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CcrlutoConsorcio)
                    .HasColumnName("CCRLUToConsorcio")
                    .HasComment("Cédula de ciudadania del representante legal, unión temporal o consorcio");

                entity.Property(e => e.CedulaRepresentanteLegal).HasComment("Cédula del representante legal");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico");

                entity.Property(e => e.CorreoRl)
                    .IsRequired()
                    .HasColumnName("CorreoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico del representante legal");

                entity.Property(e => e.CorreoRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("CorreoRLUToConsorcio")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Correo electrónico del representante legal, unión temporal o consorcio");

                entity.Property(e => e.Departamento).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.DepartamentoRl)
                    .HasColumnName("DepartamentoRL")
                    .HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.DepartamentoRlutoConsorcio)
                    .HasColumnName("DepartamentoRLUToConsorcio")
                    .HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Dirección física");

                entity.Property(e => e.DireccionRl)
                    .IsRequired()
                    .HasColumnName("DireccionRL")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Dirección física ");

                entity.Property(e => e.DireccionRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("DireccionRLUToConsorcio")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Dirección física ");

                entity.Property(e => e.EntiddaesQueIntegranLaUnionTemporal).HasComment("Cantidad de entidades que integran la unión temporal");

                entity.Property(e => e.EstaValidado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.IdentificacionTributaria).HasComment("Identificaión tributaria");

                entity.Property(e => e.Legal)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Observación del área legal");

                entity.Property(e => e.Minicipio).HasComment("Identificador de municipio referente a la tabla localización");

                entity.Property(e => e.MinicipioRlutoConsorcio)
                    .HasColumnName("MinicipioRLUToConsorcio")
                    .HasComment("Identificador de municipio referente a la tabla localización");

                entity.Property(e => e.MunucipioRl)
                    .HasColumnName("MunucipioRL")
                    .HasComment("Identificador de municipio referente a la tabla localización");

                entity.Property(e => e.NombreEntidad)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Nombre de la entidad");

                entity.Property(e => e.NombreIntegrante)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del integrante");

                entity.Property(e => e.NombreIntegrante2)
                    .HasMaxLength(200)
                    .HasComment("Nombre del integrante 2");

                entity.Property(e => e.NombreIntegrante3)
                    .HasMaxLength(200)
                    .HasComment("Nombre del integrante 3");

                entity.Property(e => e.NombreOtoConsorcio)
                    .IsRequired()
                    .HasColumnName("NombreOToConsorcio")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Nombre del consorcio");

                entity.Property(e => e.NombreProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre del proponente");

                entity.Property(e => e.NombreRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("NombreRLUToConsorcio")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Nombre del consorcio RLUT");

                entity.Property(e => e.NumeroIddentificacionProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.PorcentajeParticipacion)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("Porcentaje participación");

                entity.Property(e => e.PorcentajeParticipacion2)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("Porcentaje participación 2");

                entity.Property(e => e.PorcentajeParticipacion3)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("Porcentaje participación 3");

                entity.Property(e => e.RepresentanteLegal)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Nombre del representante legal");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número de telefono");

                entity.Property(e => e.TelefonoRl)
                    .IsRequired()
                    .HasColumnName("TelefonoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número de telefono del representante legal");

                entity.Property(e => e.TelefonoRlutoConsorcio)
                    .HasColumnName("TelefonoRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Número de telefono del representante legal, unión temporal o consorcio");

                entity.Property(e => e.TipoProponenteId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempOrdenLegibilidad)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempOrden__Archi__3EA749C6");
            });

            modelBuilder.Entity<TempProgramacion>(entity =>
            {
                entity.HasComment("Tabla utilizada para el cargue de proyectos masivos");

                entity.Property(e => e.TempProgramacionId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Descripción de la actividad");

                entity.Property(e => e.AjusteProgramacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ArchivoCargueId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.ContratoConstruccionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Duracion).HasComment("Cantidad de semanas que dura la programación");

                entity.Property(e => e.EsRutaCritica).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.EstaValidado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasComment("Fecha fin de la actividad");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasComment("Fecha inicio de la actividad");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempProgramacion)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempProgr__Archi__116A8EFB");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.TempProgramacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .HasConstraintName("FK__TempProgr__Contr__125EB334");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasComment("Tabla utilizada para el cargue de proyectos masivos");

                entity.Property(e => e.TemplateId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.Asunto)
                    .HasMaxLength(255)
                    .HasComment("Asunto de la notificación");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasComment("Cuerpo del template");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("Tipo de template");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<TemporalProyecto>(entity =>
            {
                entity.HasComment("Tabla utilizada para el cargue de proyectos masivos");

                entity.Property(e => e.TemporalProyectoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Aportante1)
                    .HasColumnName("Aportante_1")
                    .HasComment("Identificador del aportante");

                entity.Property(e => e.Aportante2)
                    .HasColumnName("Aportante_2")
                    .HasComment("Identificador del aportante");

                entity.Property(e => e.Aportante3)
                    .HasColumnName("Aportante_3")
                    .HasComment("Identificador del aportante");

                entity.Property(e => e.ArchivoCargueId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CantPrediosPostulados).HasComment("Cantidad predios postulados");

                entity.Property(e => e.Cantidad).HasComment("Cantidad de proyectos");

                entity.Property(e => e.CedulaCatastralPredio)
                    .HasMaxLength(20)
                    .HasComment("Cédula catastral del predio");

                entity.Property(e => e.CodigoDaneIe)
                    .HasColumnName("CodigoDaneIE")
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.CodigoDaneSede).HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.ConvocatoriaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.CoordinacionResponsableId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.Departamento).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.DireccionPredioPrincipal)
                    .HasMaxLength(20)
                    .HasComment("Dirección física ");

                entity.Property(e => e.DocumentoAcreditacionPredioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EnConvotatoria).HasComment("Indica si es una convocatoria");

                entity.Property(e => e.EspacioIntervenirId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.EstaValidado).HasComment("Define si cumple la caracteristica representada por el sufijo despues de la palabra es");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaSesionJunta)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de sesión de la junta");

                entity.Property(e => e.InstitucionEducativaId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.LlaveMen)
                    .IsRequired()
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(8)
                    .HasComment("Llave del MEN");

                entity.Property(e => e.Municipio).HasComment("Llave foranea a la tabla localización");

                entity.Property(e => e.NumeroActaJunta).HasComment("Número de acta de la junta");

                entity.Property(e => e.NumeroDocumentoAcreditacion)
                    .HasMaxLength(20)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.PlazoDiasInterventoria).HasComment("Plazo en dias de la interventoria");

                entity.Property(e => e.PlazoDiasObra).HasComment("Plazo en días de la obra");

                entity.Property(e => e.PlazoMesesInterventoria).HasComment("Plazo en meses de la interventoria");

                entity.Property(e => e.PlazoMesesObra).HasComment("Plazo en meses de la obra");

                entity.Property(e => e.SedeId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoAportanteId1)
                    .HasColumnName("TipoAportanteId_1")
                    .HasComment("Tipo de aportante del aportante número 1");

                entity.Property(e => e.TipoAportanteId2)
                    .HasColumnName("TipoAportanteId_2")
                    .HasComment("Tipo de aportante del aportante número 2");

                entity.Property(e => e.TipoAportanteId3)
                    .HasColumnName("TipoAportanteId_3")
                    .HasComment("Tipo de aportante del aportante número 3");

                entity.Property(e => e.TipoIntervencionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoPredioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UbicacionPredioPrincipalLatitud)
                    .IsRequired()
                    .HasColumnName("UbicacionPredioPrincipal_Latitud")
                    .HasMaxLength(10)
                    .HasComment("Latitud de la ubicación del predio principal");

                entity.Property(e => e.UbicacionPredioPrincipalLontitud)
                    .IsRequired()
                    .HasColumnName("UbicacionPredioPrincipal_Lontitud")
                    .HasMaxLength(10)
                    .HasComment("Longitud de la ubicación del predio principal");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorInterventoria)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorObra)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.ValorTotal)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.Property(e => e.VigenciaAcuerdoCofinanciacion).HasComment("Identificador de la vigencia del acuerdo de cofinanciación");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TemporalProyecto)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_archivoCargeId_ArchivoCarge_archivoCargeId");
            });

            modelBuilder.Entity<TipoActividadGestionObra>(entity =>
            {
                entity.HasComment("Almacena el tipo de actividad de una gestión de obra de los seguimientos semanales");

                entity.Property(e => e.TipoActividadGestionObraId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.SeguimientoSemanalGestionObraId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.TipoActividadGestionObra)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoActividadGestionObra_SeguimientoSemanalGestionObra");
            });

            modelBuilder.Entity<TipoDominio>(entity =>
            {
                entity.HasComment("Nombre de los grupos de las parámetricas");

                entity.Property(e => e.TipoDominioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica que el tipo de parametrica esta activo en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Descripción del Tipo de parametrica en el sistema");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del Tipo de parametrica en el sistema");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");
            });

            modelBuilder.Entity<TipoPagoConceptoPagoCriterio>(entity =>
            {
                entity.HasKey(e => e.TipoPagoCodigoConceptoPagoCriterioCodigoId)
                    .HasName("PK__TipoPago__3164A8D586395CE9");

                entity.HasComment("Almacena la relación del tipo de pago con el concepto de pago criterio");

                entity.Property(e => e.TipoPagoCodigoConceptoPagoCriterioCodigoId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.ConceptoPagoCriterioCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoPagoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasComment("Almacena los usuarios");

                entity.HasIndex(e => e.Email)
                    .HasName("Uniques_Email")
                    .IsUnique();

                entity.Property(e => e.UsuarioId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el usuario se encuentra activo en el sistema");

                entity.Property(e => e.Bloqueado).HasComment("Indica si el usuario se encuentra bloqueado por seguridad y numero de intentos fallidos en el sistema");

                entity.Property(e => e.CambiarContrasena)
                    .HasDefaultValueSql("('0')")
                    .HasComment("indica si el usuario debe cambiar la contraseña");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasComment("Contraseña del Usuario, campo cifrado");

                entity.Property(e => e.DependenciaCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Identificación de usuario definido por correo electrónico");

                entity.Property(e => e.FechaCambioPassword)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de cambio de contraseña");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaExpiracion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de expiración del usuario");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FechaUltimoIngreso)
                    .HasColumnType("datetime")
                    .HasComment("Fecha que se registra y actualiza apenas ingresa el usuario al sistema.");

                entity.Property(e => e.GrupoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.IntentosFallidos).HasComment("Cantidad de intentos de ingreso fallidos por contraseña.");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Dirección Ip del dispositivo o equipo de conexión del usuario");

                entity.Property(e => e.IpProxy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Ip proxy de la conexión del usuario");

                entity.Property(e => e.MunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.NitOrganizacion).HasMaxLength(255);

                entity.Property(e => e.NombreMaquina)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del equipo o dispositivo desde donde se esta conectando el usuario por ultima vez.");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Número que representa la entidad del sufijo después de la palabra número");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .HasComment("Observaciones de tabla en mención");

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Primer apellido usuario");

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("Primer nombre usuario");

                entity.Property(e => e.ProcedenciaCodigo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("segundo apellido usuario");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasComment("segundo nombre usuario");

                entity.Property(e => e.TelefonoCelular)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("Telefono celular usuario");

                entity.Property(e => e.TelefonoFijo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Telefono fijo usuario");

                entity.Property(e => e.TieneContratoAsignado).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TieneGrupo).HasComment("Indica que contiene el elemento de acuerdo al sufijo despues de la palabra tiene");

                entity.Property(e => e.TipoAsignacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.TipoDocumentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UrlSoporteDocumentacion)
                    .HasMaxLength(500)
                    .HasComment("URL donde se encuentra el campo en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.MunicipioId)
                    .HasConstraintName("FK_Usuario_Municipio");
            });

            modelBuilder.Entity<UsuarioPerfil>(entity =>
            {
                entity.HasComment("Almacena la relación del perfil con usuario");

                entity.Property(e => e.UsuarioPerfilId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.PerfilId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPerfil");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioPerfil)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usuario");
            });

            modelBuilder.Entity<VActualizacionPolizaYgarantias>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ActualizacionPolizaYGarantias");

                entity.Property(e => e.EstadoActualizacion)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaExpedicionActualizacionPoliza).HasColumnType("datetime");

                entity.Property(e => e.NombreContratista)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroActualizacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroPoliza)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VAjusteProgramacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_AjusteProgramacion");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EstadoCodigoNovedades)
                    .HasColumnName("estadoCodigoNovedades")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaAprobacionPoliza)
                    .HasColumnName("fechaAprobacionPoliza")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaSolictud).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("llaveMen")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContratista)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NovedadesSeleccionadas).HasColumnName("novedadesSeleccionadas");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VAportanteFuente>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Aportante_Fuente");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VAportanteFuenteUso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Aportante_Fuente_Uso");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VAportanteFuenteUsoProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Aportante_Fuente_Uso_Proyecto");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VAportantesXcriterio>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_AportantesXCriterio");
            });

            modelBuilder.Entity<VComponenteUsoNovedad>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ComponenteUsoNovedad");

                entity.Property(e => e.FaseCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoComponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAporte).HasColumnType("numeric(18, 9)");

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VCompromisoSeguimiento>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_CompromisoSeguimiento");

                entity.Property(e => e.CompromisoSeguimientoId).ValueGeneratedOnAdd();

                entity.Property(e => e.DescripcionSeguimiento)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCompromisoCodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VConceptosUsosXsolicitudPagoId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ConceptosUsosXSolicitudPagoId");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ConceptoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsoNombre)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<VConfinanciacionReporte>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ConfinanciacionReporte");

                entity.Property(e => e.FechaAcuerdo).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.IdPadre)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocalizacionId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoAportante).HasMaxLength(250);

                entity.Property(e => e.ValorDocumento).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VContratacionProyectoSolicitudLiquidacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ContratacionProyectoSolicitudLiquidacion");

                entity.Property(e => e.EstadoAprobacionLiquidacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoAprobacionLiquidacionString)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoTramiteLiquidacion)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoTramiteLiquidacionString)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoValidacionLiquidacionCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoValidacionLiquidacionString)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaAprobacionLiquidacion).HasColumnType("datetime");

                entity.Property(e => e.FechaPoliza).HasColumnType("datetime");

                entity.Property(e => e.FechaTramiteLiquidacionControl).HasColumnType("datetime");

                entity.Property(e => e.FechaValidacionLiquidacion).HasColumnType("date");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitudLiquidacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorSolicitud).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VContratistaReporteHist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ContratistaReporteHist");

                entity.Property(e => e.ContratistaId).ValueGeneratedOnAdd();

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroInvitacion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcesoSeleccionProponenteId).HasColumnName("ProcesoSeleccionProponenteID");

                entity.Property(e => e.RepresentanteLegal)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RepresentanteLegalNumeroIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VContratoPagosRealizados>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Contrato_Pagos_Realizados");

                entity.Property(e => e.FaseContrato)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeFacturado).HasColumnType("decimal(38, 16)");

                entity.Property(e => e.PorcentajePorPagar).HasColumnType("decimal(38, 16)");

                entity.Property(e => e.SaldoPorPagar).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.ValorSolicitud).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VContratosXcontratacionProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ContratosXContratacionProyecto");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VCuentaBancariaPago>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_CuentaBancariaPago");

                entity.Property(e => e.NumeroCuentaBanco)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorNetoGiro).HasColumnType("decimal(25, 3)");
            });

            modelBuilder.Entity<VDefensaJudicialContratacionProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DefensaJudicialContratacionProyecto");
            });

            modelBuilder.Entity<VDescuentoTecnicaXordenGiro>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentoTecnicaXOrdenGiro");
            });

            modelBuilder.Entity<VDescuentosFinancieraOdgxFuenteFinanciacionXaportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosFinancieraODGX_FuenteFinanciacionXAportante");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VDescuentosOdgxFuenteFinanciacionXaportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosODGX_FuenteFinanciacionXAportante");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.TipoRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VDescuentosXordenGiro>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosXOrdenGiro");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VDescuentosXordenGiroAportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosXOrdenGiroAportante");
            });

            modelBuilder.Entity<VDescuentosXordenGiroXproyectoXaportanteXconcepto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosXOrdenGiroXProyectoXAportanteXConcepto");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosXOrdenGiroXProyectoXAportanteXConceptoXUso");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Uso)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(25, 0)");
            });

            modelBuilder.Entity<VDescuentosXordenGiroXproyectoXfaseXaportanteXconcepto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DescuentosXOrdenGiroXProyectoXFaseXAportanteXConcepto");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CriterioCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EsPreconstruccion).HasColumnName("esPreconstruccion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDescuentoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(25, 0)");
            });

            modelBuilder.Entity<VDisponibilidadPresupuestal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DisponibilidadPresupuestal");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDdp)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudEspecialCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VDominio>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Dominio");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreation).HasColumnType("datetime");

                entity.Property(e => e.IdValor)
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NombreDominio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VDrpNovedadXfaseContratacionId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpNovedadXFaseContratacionId");

                entity.Property(e => e.ValorDrp).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VDrpXcontratacionXproyectoXaportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXContratacionXProyectoXAportante");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAporte).HasColumnType("numeric(38, 9)");
            });

            modelBuilder.Entity<VDrpXcontratacionXproyectoXaportanteXfaseXcriterioXconceptoXusos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXContratacionXProyectoXAportanteXFaseXCriterioXConceptoXUsos");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ConceptoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Facturado).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Saldo).HasColumnType("numeric(38, 0)");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VDrpXcontratacionXproyectoXfaseXconceptoXusos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXContratacionXProyectoXFaseXConceptoXUsos");

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Saldo).HasColumnType("numeric(21, 2)");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VDrpXcontratoXaportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXContratoXAportante");

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VDrpXfaseContratacionId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXFaseContratacionId");

                entity.Property(e => e.ValorDrp).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VDrpXfaseXcontratacionIdXnovedad>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXFaseXContratacionIdXNovedad");

                entity.Property(e => e.ValorDrp).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VDrpXproyectoXusos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DrpXProyectoXUsos");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Saldo).HasColumnType("numeric(21, 2)");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VEjecucionFinancieraXcontrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_EjecucionFinancieraXContrato");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoTesoral).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorNeto).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorSolicitudDdp)
                    .HasColumnName("ValorSolicitudDDP")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VEjecucionFinancieraXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_EjecucionFinancieraXProyecto");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.OrdenadoGirarAntesImpuestos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PorcentajeEjecucionFinanciera).HasColumnType("decimal(38, 15)");

                entity.Property(e => e.Saldo).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TotalComprometido).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VEjecucionPresupuestalXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_EjecucionPresupuestalXProyecto");

                entity.Property(e => e.FacturadoAntesImpuestos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.PorcentajeEjecucionPresupuestal).HasColumnType("decimal(38, 16)");

                entity.Property(e => e.Saldo).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TotalComprometido).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VFacturadoOdgXcontratacionXproyectoXfaseXconceptoXaportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FacturadoOdgXContratacionXProyectoXFaseXConceptoXAportante");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EsPreconstruccion).HasColumnName("esPreconstruccion");

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VFacturadoXodgXcontratacionXproyectoXaportanteXfaseXconcepXuso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FacturadoXOdgXContratacionXProyectoXAportanteXFaseXConcepXUso");

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EsPreconstruccion).HasColumnName("esPreconstruccion");

                entity.Property(e => e.SaldoUso).HasColumnType("numeric(38, 0)");

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VFuentesUsoXcontratoId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FuentesUsoXContratoId");

                entity.Property(e => e.FuenteFinanciacion).HasMaxLength(250);

                entity.Property(e => e.NombreUso).HasMaxLength(250);

                entity.Property(e => e.TipoUso)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VFuentesUsoXcontratoIdXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_FuentesUsoXContratoIdXProyecto");

                entity.Property(e => e.FuenteFinanciacion).HasMaxLength(250);

                entity.Property(e => e.NombreUso).HasMaxLength(250);

                entity.Property(e => e.TipoUso)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VGestionarGarantiasPolizas>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GestionarGarantiasPolizas");

                entity.Property(e => e.EstadoPoliza).HasMaxLength(250);

                entity.Property(e => e.EstadoPolizaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacionContrato).HasColumnType("datetime");

                entity.Property(e => e.FechaFirma).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitudContratacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegistroCompletoNombre)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegistroCompletoPolizaNombre)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitud)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigoContratacion)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudContratacion).HasMaxLength(250);
            });

            modelBuilder.Entity<VListCompromisosComiteTecnico>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ListCompromisosComiteTecnico");

                entity.Property(e => e.Compromiso)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaComite).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

                entity.Property(e => e.NumeroComite)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VListCompromisosTemas>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ListCompromisosTemas");

                entity.Property(e => e.Compromiso)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaComite).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

                entity.Property(e => e.NumeroComite)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VListaContratacionModificacionContractual>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ListaContratacionModificacionContractual");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoDelRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitud)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VListaProyectos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ListaProyectos");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoJuridicoPredios).HasMaxLength(250);

                entity.Property(e => e.EstadoProyectoInterventoria).HasMaxLength(250);

                entity.Property(e => e.EstadoProyectoObra).HasMaxLength(250);

                entity.Property(e => e.EstadoRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VNombreCuentaXodgXaportanteXconcepto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_NombreCuentaXOdgXAportanteXConcepto");

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NombreBanco)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NombreCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VNovedadContractual>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_NovedadContractual");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCodigoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoDescripcion).HasMaxLength(250);

                entity.Property(e => e.FechaSolictud).HasColumnType("datetime");

                entity.Property(e => e.FechaValidacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVerificacion).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContratista)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NovedadesSeleccionadas).HasColumnName("novedadesSeleccionadas");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VNovedadContractualReporteHist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_NovedadContractualReporteHist");

                entity.Property(e => e.CausaRechazo).IsUnicode(false);

                entity.Property(e => e.EsAplicadaAcontrato).HasColumnName("EsAplicadaAContrato");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProcesoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacionGestionContractual).HasColumnType("date");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioActaApoyo).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioActaContratistaInterventoria).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioActaContratistaObra).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioActaSupervisor).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioFirmaContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioFirmaFiduciaria).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioGestionContractual).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaObra).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaApoyo).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContratistaInterventoria).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaFiduciaria).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaSupervisor).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSesionInstancia).HasColumnType("datetime");

                entity.Property(e => e.FechaSolictud).HasColumnType("datetime");

                entity.Property(e => e.FechaTramiteGestionar).HasColumnType("datetime");

                entity.Property(e => e.FechaValidacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVerificacion).HasColumnType("datetime");

                entity.Property(e => e.InstanciaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NovedadContractualId).ValueGeneratedOnAdd();

                entity.Property(e => e.NumeroOtroSi)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RazonesNoContinuaProceso).IsUnicode(false);

                entity.Property(e => e.UrlDocumentoSuscrita).IsUnicode(false);

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporteFirmas).IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(400);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(400);
            });

            modelBuilder.Entity<VOrdenGiro>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_OrdenGiro");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoNombre2)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaAprobacionFinanciera).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaPagoFiduciaria).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoAprobar).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroCompletoTramitar).HasColumnType("datetime");

                entity.Property(e => e.IntEstadoCodigo).HasColumnName("intEstadoCodigo");

                entity.Property(e => e.Modalidad)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitudOrdenGiro)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitudPago)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitud)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VOrdenGiroPagosXusoAportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_OrdenGiroPagosXUsoAportante");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VOrdenGiroPagosXusoAportanteXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_OrdenGiroPagosXUsoAportanteXProyecto");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VOrdenGiroXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_OrdenGiroXProyecto");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumeroOrdenGiro)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VPagosSolicitudXcontratacionXproyectoXuso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PagosSolicitudXContratacionXProyectoXUso");

                entity.Property(e => e.ConceptoPagoCriterio)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstaAprobadaOdg).HasColumnName("EstaAprobadaODG");

                entity.Property(e => e.NumeroDrp)
                    .IsRequired()
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoUso).HasColumnType("decimal(30, 0)");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Uso).HasMaxLength(250);
            });

            modelBuilder.Entity<VPagosSolicitudXodgXcontratacionXproyectoXuso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PagosSolicitudXOdgXContratacionXProyectoXUso");

                entity.Property(e => e.NumeroDrp)
                    .IsRequired()
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoUso).HasColumnType("decimal(30, 0)");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Uso)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<VParametricas>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Parametricas");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(31)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDominioId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VPermisosMenus>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PermisosMenus");

                entity.Property(e => e.RutaFormulario)
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VPlantillaOrdenGiro>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PlantillaOrdenGiro");

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Concepto).HasMaxLength(250);

                entity.Property(e => e.ConsecutivoFfie)
                    .IsRequired()
                    .HasColumnName("ConsecutivoFFIE")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.DescuentoAns).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.DescuentoOtros).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.DescuentoReteFuente).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.FormaPago).HasMaxLength(250);

                entity.Property(e => e.IdentificacionTercero)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreBanco).HasMaxLength(250);

                entity.Property(e => e.NombreBancoTercero).HasMaxLength(250);

                entity.Property(e => e.NombreCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuentaTercero)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Numerofactura)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TerceroCausasionIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TerceroCausasionNombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TitularTercero)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorConcepto).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.ValorNetoGiro).HasColumnType("decimal(25, 3)");
            });

            modelBuilder.Entity<VPlantillaOrdenGiroUsos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_PlantillaOrdenGiroUsos");

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Concepto).HasMaxLength(250);

                entity.Property(e => e.ConsecutivoFfie)
                    .IsRequired()
                    .HasColumnName("ConsecutivoFFIE")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.DescuentoAns).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.DescuentoOtros).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.DescuentoReteFuente).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.FormaPago).HasMaxLength(250);

                entity.Property(e => e.IdentificacionTercero)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreBanco).HasMaxLength(250);

                entity.Property(e => e.NombreBancoTercero).HasMaxLength(250);

                entity.Property(e => e.NombreCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuentaTercero)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Numerofactura)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TerceroCausasionIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TerceroCausasionNombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TitularTercero)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ValorConcepto).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.ValorNetoGiro).HasColumnType("decimal(25, 3)");
            });

            modelBuilder.Entity<VProcesoSeleccionIntegrante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProcesoSeleccionIntegrante");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreIntegrante)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ProcesoSeleccionIntegranteId).ValueGeneratedOnAdd();

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VProcesoSeleccionReporteHist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProcesoSeleccionReporteHist");

                entity.Property(e => e.AlcanceParticular)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CriteriosSeleccion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroProceso)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ProcesoSeleccionId).ValueGeneratedOnAdd();

                entity.Property(e => e.TipoAlcanceCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoOrdenEligibilidadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporteEvaluacion)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporteProponentesSeleccionados)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VProgramacionBySeguimientoSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProgramacionBySeguimientoSemanal");

                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VProyectoReporteHist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProyectoReporteHist");

                entity.Property(e => e.CoordinacionResponsableCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProgramacionCodigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProyectoCodigoOld)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProyectoInterventoriaCodigo).HasMaxLength(100);

                entity.Property(e => e.EstadoProyectoObraCodigo).HasMaxLength(100);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSesionJunta).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProyectoId).ValueGeneratedOnAdd();

                entity.Property(e => e.TipoIntervencionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPredioCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlMonitoreo)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorInterventoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorObra).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotal).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VProyectosBalance>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProyectosBalance");

                entity.Property(e => e.BalanceFinancieroId).HasColumnName("BalanceFInancieroId");

                entity.Property(e => e.EstadoBalance).HasMaxLength(250);

                entity.Property(e => e.EstadoBalanceCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTerminacionProyecto)
                    .HasColumnName("fechaTerminacionProyecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasColumnName("institucionEducativa")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SedeEducativa)
                    .HasColumnName("sedeEducativa")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .IsRequired()
                    .HasColumnName("tipoIntervencion")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<VProyectosCierre>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProyectosCierre");

                entity.Property(e => e.EstadoInforme).HasMaxLength(250);

                entity.Property(e => e.EstadoInformeCod)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaTerminacionInterventoria)
                    .HasColumnName("fechaTerminacionInterventoria")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaTerminacionObra)
                    .HasColumnName("fechaTerminacionObra")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasColumnName("institucionEducativa")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SedeEducativa)
                    .IsRequired()
                    .HasColumnName("sedeEducativa")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .IsRequired()
                    .HasColumnName("tipoIntervencion")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<VProyectosXcontrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProyectosXContrato");

                entity.Property(e => e.Departamento)
                    .HasColumnName("departamento")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActaFase2)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.FechaActaInicioFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaRegistroProyecto)
                    .HasColumnName("fechaRegistroProyecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasColumnName("institucionEducativa")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasColumnName("municipio")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContratista)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasColumnName("sede")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .IsRequired()
                    .HasColumnName("tipoIntervencion")
                    .HasMaxLength(250);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorTotal).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VRegistrarAvanceSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarAvanceSemanal");

                entity.Property(e => e.EstadoObra).HasMaxLength(250);

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaUltimoReporte)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModalidadCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContrato)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VRegistrarAvanceSemanalCompletos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarAvanceSemanalCompletos");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            });

            modelBuilder.Entity<VRegistrarAvanceSemanalNew>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarAvanceSemanalNew");

                entity.Property(e => e.ContratoId).HasColumnName("contratoId");

                entity.Property(e => e.EstadoObra).HasMaxLength(250);

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaUltimoReporte)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VRegistrarFase1>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarFase1");

                entity.Property(e => e.CantidadProyectosAsociados).HasColumnName("cantidadProyectosAsociados");

                entity.Property(e => e.CantidadProyectosConPerfilesPendientes).HasColumnName("cantidadProyectosConPerfilesPendientes");

                entity.Property(e => e.CantidadProyectosRequisitosAprobados).HasColumnName("cantidadProyectosRequisitosAprobados");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRegistrarLiquidacionContrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarLiquidacionContrato");

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NombreEstado).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContrato)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRegistrarPersonalObra>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarPersonalObra");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProgramacionInicial)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoProgramacionInicialCodigo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFirmaActaInicio).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VRendimientodXcuentaBancaria>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RendimientodXCuentaBancaria");

                entity.Property(e => e.CuentaBancaria)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RendimientoIncorporar).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TotalRendimientos).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<VReporteProyectos>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ReporteProyectos");

                entity.Property(e => e.EstadoProyectoObra).HasMaxLength(250);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProyecto).HasMaxLength(250);
            });

            modelBuilder.Entity<VRequisitosTecnicosConstruccionAprobar>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RequisitosTecnicosConstruccionAprobar");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaActaInicioFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionApoyo).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRequisitosTecnicosInicioConstruccion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RequisitosTecnicosInicioConstruccion");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EstadoNombreVerificacion)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaActaInicioFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionInterventor).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRequisitosTecnicosPreconstruccion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RequisitosTecnicosPreconstruccion");

                entity.Property(e => e.CantidadProyectosAsociados).HasColumnName("cantidadProyectosAsociados");

                entity.Property(e => e.CantidadProyectosConPerfilesPendientes).HasColumnName("cantidadProyectosConPerfilesPendientes");

                entity.Property(e => e.CantidadProyectosRequisitosAprobados).HasColumnName("cantidadProyectosRequisitosAprobados");

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRpsPorContratacion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RpsPorContratacion");

                entity.Property(e => e.EsNovedad).HasColumnName("esNovedad");

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorSolicitud).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VSaldoPresupuestalXcontrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SaldoPresupuestalXContrato");

                entity.Property(e => e.SaldoPresupuestalObraInterventoria).HasColumnType("decimal(20, 0)");

                entity.Property(e => e.SaldoPresupuestalOtrosCostos).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.ValorDdpobraInterventoria)
                    .HasColumnName("ValorDDPObraInterventoria")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ValorDdpotrosCostos)
                    .HasColumnName("ValorDDPOtrosCostos")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ValorFacturadoObraInterventoria).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.ValorFacturadoOtrosCostos).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VSaldoPresupuestalXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SaldoPresupuestalXProyecto");

                entity.Property(e => e.SaldoPresupuestal).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.ValorDdp)
                    .HasColumnName("ValorDDP")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ValorFacturado).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VSaldosFuenteXaportanteId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SaldosFuenteXAportanteId");

                entity.Property(e => e.ComprometidoEnDdp).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.RendimientosIncorporados).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.SaldoActual).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VSaldosFuenteXaportanteIdValidar>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SaldosFuenteXAportanteIdValidar");

                entity.Property(e => e.ComprometidoEnDdp).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.RendimientosIncorporados).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.SaldoActual).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VSeguimientoSemanalRegistrar>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SeguimientoSemanalRegistrar");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            });

            modelBuilder.Entity<VSesionParticipante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SesionParticipante");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(601)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("date");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(601)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSetHistDefensaJudicial>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_setHistDefensaJudicial");

                entity.Property(e => e.CanalIngresoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CuantiaPerjuicios).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DefensaJudicialId).ValueGeneratedOnAdd();

                entity.Property(e => e.EsDemandaFfie).HasColumnName("EsDemandaFFIE");

                entity.Property(e => e.EstadoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRadicadoFfie)
                    .HasColumnName("FechaRadicadoFFIE")
                    .HasColumnType("datetime");

                entity.Property(e => e.JurisdiccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LegitimacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroProceso)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoFfie)
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoAccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporteProceso)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSetHistDefensaJudicialContratacionProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_setHistDefensaJudicialContratacionProyecto");

                entity.Property(e => e.DefensaJudicialContratacionProyectoId).ValueGeneratedOnAdd();

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSetHistProyectoAportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_setHistProyectoAportante");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.ProyectoAportanteId).ValueGeneratedOnAdd();

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorInterventoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorObra).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotalAportante).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VSolicitudPago>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SolicitudPago");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombre).HasMaxLength(250);

                entity.Property(e => e.EstadoNombre2).HasMaxLength(250);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.IntEstadoCodigo).HasColumnName("intEstadoCodigo");

                entity.Property(e => e.ModalidadNombre).HasMaxLength(250);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VTablaOdgDescuento>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_TablaOdgDescuento");

                entity.Property(e => e.ConceptoPago)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Descuento)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.DescuentoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescuento).HasColumnType("decimal(25, 0)");
            });

            modelBuilder.Entity<VTablaOdgFacturado>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_TablaOdgFacturado");

                entity.Property(e => e.ConceptoPago).HasMaxLength(250);

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Descuentos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TipoPago).HasMaxLength(250);

                entity.Property(e => e.Uso)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ValorFacturado).HasColumnType("decimal(38, 0)");
            });

            modelBuilder.Entity<VTablaOdgOtroDescuento>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_TablaOdgOtroDescuento");

                entity.Property(e => e.ConceptoPago).HasMaxLength(250);

                entity.Property(e => e.Descuento).HasMaxLength(250);

                entity.Property(e => e.DescuentoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VTotalComprometidoXcontratacionProyectoTipoSolicitud>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_TotalComprometidoXContratacionProyectoTipoSolicitud");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TotalComprometido).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VUbicacionXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_UbicacionXProyecto");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DepartamentoId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.MunicipioId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VUsosXsolicitudPago>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_UsosXSolicitudPago");

                entity.Property(e => e.NombreUso)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VUsuarioPerfil>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_UsuarioPerfil");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioPerfilId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VUsuarioRol>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Usuario_Rol");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ProcedenciaCodigo).HasMaxLength(250);

                entity.Property(e => e.Rol)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VValidarSeguimientoSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValidarSeguimientoSemanal");

                entity.Property(e => e.EstadoMuestras).HasMaxLength(250);

                entity.Property(e => e.EstadoObra).HasMaxLength(250);

                entity.Property(e => e.EstadoSeguimientoSemanal).HasMaxLength(250);

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaReporte).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.IntestadoSeguimientoSemanalCodigo).HasColumnName("INTEstadoSeguimientoSemanalCodigo");

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VValorConstruccionXproyectoContrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorConstruccionXProyectoContrato");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ValorConstruccion)
                    .HasColumnName("valorConstruccion")
                    .HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorFacturadoContrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoContrato");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoPresupuestal).HasColumnType("decimal(38, 0)");

                entity.Property(e => e.ValorSolicitudDdp)
                    .HasColumnName("ValorSolicitudDDP")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VValorFacturadoContratoXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoContratoXProyecto");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoPresupuestal).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorSolicitudDdp)
                    .HasColumnName("ValorSolicitudDDP")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VValorFacturadoContratoXproyectoXuso>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoContratoXProyectoXUso");

                entity.Property(e => e.Concepto)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ConceptoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoPresupuestal).HasColumnType("decimal(31, 0)");

                entity.Property(e => e.Uso)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UsoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsoDrpCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFacturado).HasColumnType("decimal(30, 0)");

                entity.Property(e => e.ValorSolicitudDdp)
                    .HasColumnName("ValorSolicitudDDP")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VValorFacturadoProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoProyecto");

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorFacturadoSolicitudPago>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoSolicitudPago");

                entity.Property(e => e.Valor).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorFacturadoXfasesSolicitudPago>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorFacturadoXFasesSolicitudPago");

                entity.Property(e => e.ValorFacturado).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorTrasladoXproyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorTrasladoXProyecto");

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ValorTraslado).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorUsoXcontratoAportante>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorUsoXContratoAportante");

                entity.Property(e => e.ConceptoPagoCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FaseId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VValorUsoXcontratoId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorUsoXContratoId");

                entity.Property(e => e.FaseId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(38, 2)");
            });

            modelBuilder.Entity<VValorUsosFasesAportanteProyecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValorUsosFasesAportanteProyecto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<VVerificarSeguimientoSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_VerificarSeguimientoSemanal");

                entity.Property(e => e.EstadoMuestras).HasMaxLength(250);

                entity.Property(e => e.EstadoObra).HasMaxLength(250);

                entity.Property(e => e.EstadoSeguimientoSemanal).HasMaxLength(250);

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaReporte).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion).HasMaxLength(250);
            });

            modelBuilder.Entity<VigenciaAporte>(entity =>
            {
                entity.HasComment("Almacena las vigencias del aporte relacionados a la fuente de financiación");

                entity.Property(e => e.VigenciaAporteId).HasComment("Llave primaria de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el registro ha sido eliminado si tiene valor 1");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del registro");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha en que se modifica registro");

                entity.Property(e => e.FuenteFinanciacionId).HasComment("Llave foranea a la tabla en mención");

                entity.Property(e => e.TipoVigenciaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Relación a la tabla dominio con la columna código");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario creación");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("usuario que realiza modificación del registro");

                entity.Property(e => e.ValorAporte)
                    .HasColumnType("numeric(18, 2)")
                    .HasComment("Representa la cantidad de dinero del campo correspondiente");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.VigenciaAporte)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_VigenciaAporte_FuenteFinanciacion");
            });

            modelBuilder.HasSequence<int>("ConsecutivoActaRendimientos");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
