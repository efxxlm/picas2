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
        public virtual DbSet<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual DbSet<ArchivoCargue> ArchivoCargue { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<AvanceFisicoFinanciero> AvanceFisicoFinanciero { get; set; }
        public virtual DbSet<CargueObservacion> CargueObservacion { get; set; }
        public virtual DbSet<Cofinanciacion> Cofinanciacion { get; set; }
        public virtual DbSet<CofinanciacionAportante> CofinanciacionAportante { get; set; }
        public virtual DbSet<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
        public virtual DbSet<ComiteTecnico> ComiteTecnico { get; set; }
        public virtual DbSet<ComponenteAportante> ComponenteAportante { get; set; }
        public virtual DbSet<ComponenteUso> ComponenteUso { get; set; }
        public virtual DbSet<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
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
        public virtual DbSet<ControlRecurso> ControlRecurso { get; set; }
        public virtual DbSet<ControversiaActuacion> ControversiaActuacion { get; set; }
        public virtual DbSet<ControversiaActuacionMesa> ControversiaActuacionMesa { get; set; }
        public virtual DbSet<ControversiaActuacionMesaSeguimiento> ControversiaActuacionMesaSeguimiento { get; set; }
        public virtual DbSet<ControversiaContractual> ControversiaContractual { get; set; }
        public virtual DbSet<ControversiaMotivo> ControversiaMotivo { get; set; }
        public virtual DbSet<CronogramaSeguimiento> CronogramaSeguimiento { get; set; }
        public virtual DbSet<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual DbSet<DefensaJudicial> DefensaJudicial { get; set; }
        public virtual DbSet<DefensaJudicialContratacionProyecto> DefensaJudicialContratacionProyecto { get; set; }
        public virtual DbSet<DefensaJudicialSeguimiento> DefensaJudicialSeguimiento { get; set; }
        public virtual DbSet<DemandadoConvocado> DemandadoConvocado { get; set; }
        public virtual DbSet<DemandanteConvocante> DemandanteConvocante { get; set; }
        public virtual DbSet<DisponibilidadPresupuestal> DisponibilidadPresupuestal { get; set; }
        public virtual DbSet<DisponibilidadPresupuestalObservacion> DisponibilidadPresupuestalObservacion { get; set; }
        public virtual DbSet<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
        public virtual DbSet<DocumentoApropiacion> DocumentoApropiacion { get; set; }
        public virtual DbSet<Dominio> Dominio { get; set; }
        public virtual DbSet<EnsayoLaboratorioMuestra> EnsayoLaboratorioMuestra { get; set; }
        public virtual DbSet<FaseComponenteUso> FaseComponenteUso { get; set; }
        public virtual DbSet<FichaEstudio> FichaEstudio { get; set; }
        public virtual DbSet<FlujoInversion> FlujoInversion { get; set; }
        public virtual DbSet<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual DbSet<GestionFuenteFinanciacion> GestionFuenteFinanciacion { get; set; }
        public virtual DbSet<GestionObraCalidadEnsayoLaboratorio> GestionObraCalidadEnsayoLaboratorio { get; set; }
        public virtual DbSet<GrupoMunicipios> GrupoMunicipios { get; set; }
        public virtual DbSet<InfraestructuraIntervenirProyecto> InfraestructuraIntervenirProyecto { get; set; }
        public virtual DbSet<InstitucionEducativaSede> InstitucionEducativaSede { get; set; }
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
        public virtual DbSet<NovedadContractual> NovedadContractual { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Plantilla> Plantilla { get; set; }
        public virtual DbSet<PolizaGarantia> PolizaGarantia { get; set; }
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
        public virtual DbSet<Programacion> Programacion { get; set; }
        public virtual DbSet<ProgramacionPersonalContrato> ProgramacionPersonalContrato { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<ProyectoAdministrativo> ProyectoAdministrativo { get; set; }
        public virtual DbSet<ProyectoAdministrativoAportante> ProyectoAdministrativoAportante { get; set; }
        public virtual DbSet<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual DbSet<ProyectoFuentes> ProyectoFuentes { get; set; }
        public virtual DbSet<ProyectoMonitoreoWeb> ProyectoMonitoreoWeb { get; set; }
        public virtual DbSet<ProyectoPredio> ProyectoPredio { get; set; }
        public virtual DbSet<ProyectoRequisitoTecnico> ProyectoRequisitoTecnico { get; set; }
        public virtual DbSet<RegistroPresupuestal> RegistroPresupuestal { get; set; }
        public virtual DbSet<RequisitoTecnicoRadicado> RequisitoTecnicoRadicado { get; set; }
        public virtual DbSet<SeguimientoActuacionDerivada> SeguimientoActuacionDerivada { get; set; }
        public virtual DbSet<SeguimientoDiario> SeguimientoDiario { get; set; }
        public virtual DbSet<SeguimientoDiarioObservaciones> SeguimientoDiarioObservaciones { get; set; }
        public virtual DbSet<SeguimientoSemanal> SeguimientoSemanal { get; set; }
        public virtual DbSet<SeguimientoSemanalAvanceFinanciero> SeguimientoSemanalAvanceFinanciero { get; set; }
        public virtual DbSet<SeguimientoSemanalAvanceFisico> SeguimientoSemanalAvanceFisico { get; set; }
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
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<VGestionarGarantiasPolizas> VGestionarGarantiasPolizas { get; set; }
        public virtual DbSet<VListaProyectos> VListaProyectos { get; set; }
        public virtual DbSet<VProyectosXcontrato> VProyectosXcontrato { get; set; }
        public virtual DbSet<VRegistrarAvanceSemanal> VRegistrarAvanceSemanal { get; set; }
        public virtual DbSet<VRegistrarFase1> VRegistrarFase1 { get; set; }
        public virtual DbSet<VRegistrarPersonalObra> VRegistrarPersonalObra { get; set; }
        public virtual DbSet<VRequisitosTecnicosConstruccionAprobar> VRequisitosTecnicosConstruccionAprobar { get; set; }
        public virtual DbSet<VRequisitosTecnicosInicioConstruccion> VRequisitosTecnicosInicioConstruccion { get; set; }
        public virtual DbSet<VRequisitosTecnicosPreconstruccion> VRequisitosTecnicosPreconstruccion { get; set; }
        public virtual DbSet<VValidarSeguimientoSemanal> VValidarSeguimientoSemanal { get; set; }
        public virtual DbSet<VVerificarSeguimientoSemanal> VVerificarSeguimientoSemanal { get; set; }
        public virtual DbSet<VigenciaAporte> VigenciaAporte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActuacionSeguimiento>(entity =>
            {
                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoDerivadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoReclamacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacionAdelantada).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.ActuacionSeguimiento)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActuacionSeguimiento_ControversiaActuacion");
            });

            modelBuilder.Entity<AportanteFuenteFinanciacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEdicion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioEdicion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.ReferenciaId).HasColumnName("ReferenciaID");

                entity.Property(e => e.Ruta)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Tamano)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(255);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(255);
            });

            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Observacion).HasMaxLength(500);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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

            modelBuilder.Entity<AvanceFisicoFinanciero>(entity =>
            {
                entity.Property(e => e.Causa).HasMaxLength(500);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaReporte).HasColumnType("datetime");

                entity.Property(e => e.Observaciones).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VariableCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CargueObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstruccionCargue)
                    .WithMany(p => p.CargueObservacion)
                    .HasForeignKey(d => d.ConstruccionCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CargueObservacion_ConstruccionCargue");
            });

            modelBuilder.Entity<Cofinanciacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);
            });

            modelBuilder.Entity<CofinanciacionAportante>(entity =>
            {
                entity.HasIndex(e => new { e.CofinanciacionId, e.Eliminado })
                    .HasName("idxconfid_eliminado");

                entity.Property(e => e.CuentaConRp).HasColumnName("CuentaConRP");

                entity.Property(e => e.DepartamentoId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.MunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);

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
                entity.Property(e => e.FechaActa).HasColumnType("datetime");

                entity.Property(e => e.FechaAcuerdo).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);

                entity.Property(e => e.ValorDocumento).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.ValorTotalAportante).HasColumnType("numeric(38, 2)");

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
                entity.Property(e => e.EstadoActaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoComiteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAplazamiento).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaOrdenDia).HasColumnType("datetime");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroComite)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaSesion)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporteVotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoTemaFiduciarioCodigo)
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

            modelBuilder.Entity<ComponenteAportante>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FaseCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoComponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratacionProyectoAportante)
                    .WithMany(p => p.ComponenteAportante)
                    .HasForeignKey(d => d.ContratacionProyectoAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteAportante_ContratacionProyectoAportante");
            });

            modelBuilder.Entity<ComponenteUso>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoUsoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ComponenteAportante)
                    .WithMany(p => p.ComponenteUso)
                    .HasForeignKey(d => d.ComponenteAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteUso_ComponenteAportante");
            });

            modelBuilder.Entity<CompromisoSeguimiento>(entity =>
            {
                entity.Property(e => e.DescripcionSeguimiento)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCompromisoCodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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

            modelBuilder.Entity<ConstruccionCargue>(entity =>
            {
                entity.Property(e => e.EstadoCargueCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCargue).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.TipoCargueCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionCargue)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionCargue_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.TipoObservacionConstruccion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionObservacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .HasConstraintName("FK_ConstruccionObservacion_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionPerfil>(entity =>
            {
                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroRadicadoFfie)
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoFfie1)
                    .HasColumnName("NumeroRadicadoFFIE1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoFfie2)
                    .HasColumnName("NumeroRadicadoFFIE2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoFfie3)
                    .HasColumnName("NumeroRadicadoFFIE3")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.PerfilCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.ConstruccionPerfil)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionPerfil_ContratoConstruccion");
            });

            modelBuilder.Entity<ConstruccionPerfilNumeroRadicado>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroRadicado).HasMaxLength(50);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstruccionPerfil)
                    .WithMany(p => p.ConstruccionPerfilNumeroRadicado)
                    .HasForeignKey(d => d.ConstruccionPerfilId)
                    .HasConstraintName("FK_ConstruccionPerfilNumeroRadicado_ConstruccionPerfil");
            });

            modelBuilder.Entity<ConstruccionPerfilObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoObservacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ConstruccionPerfil)
                    .WithMany(p => p.ConstruccionPerfilObservacion)
                    .HasForeignKey(d => d.ConstruccionPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstruccionPerfilObservacion_ConstruccionPerfil");
            });

            modelBuilder.Entity<Contratacion>(entity =>
            {
                entity.Property(e => e.ConsideracionDescripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioDocumentacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTramite).HasColumnType("datetime");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaMinuta)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contratista)
                    .WithMany(p => p.Contratacion)
                    .HasForeignKey(d => d.ContratistaId)
                    .HasConstraintName("FK_Contratacion_Contratista");
            });

            modelBuilder.Entity<ContratacionObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoObraCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoRequisitosVerificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacionRequisitos).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVigencia).HasColumnType("datetime");

                entity.Property(e => e.NumeroLicencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeAvanceObra)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaCargaActaTerminacionContrato).HasMaxLength(200);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAporte).HasColumnType("numeric(18, 2)");

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
                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

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

                entity.HasOne(d => d.ProcesoSeleccionProponente)
                    .WithMany(p => p.Contratista)
                    .HasForeignKey(d => d.ProcesoSeleccionProponenteId)
                    .HasConstraintName("FK_Contratista_ProcesoSeleccionProponente");
            });

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.Property(e => e.EstadoActa)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActaFase2)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.EstadoDocumentoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoFase2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoVerificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoVerificacionConstruccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActaInicioFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaActaInicioFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitos).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosApoyo).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionApoyo).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionInterventor).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionSupervisor).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosInterventor).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosSupervisor).HasColumnType("datetime");

                entity.Property(e => e.FechaCambioEstadoFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioFirma).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoria).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoriaFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaActaContratistaInterventoriaFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContrato).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaFiduciaria).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTerminacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTerminacionFase2).HasColumnType("datetime");

                entity.Property(e => e.FechaTramite).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActa)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaSuscrita)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaDocumento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contrato_Contratacion");
            });

            modelBuilder.Entity<ContratoConstruccion>(entity =>
            {
                entity.Property(e => e.ActaApropiacionFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ActaApropiacionFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ActaApropiacionObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Administracion).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.AprovechamientoForestalApropiacionFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.AprovechamientoForestalFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.AprovechamientoForestalObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.AseguramientoCalidadFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.AseguramientoCalidadFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.AseguramientoCalidadObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CambioFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.CambioFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.CambioObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CostoDirecto).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Imprevistos).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.InventarioArboreoFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.InventarioArboreoFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.InventarioArboreoObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.LicenciaFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.LicenciaFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.LicenciaObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ManejoAguasLluviasFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ManejoAguasLluviasFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ManejoAguasLluviasObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ManejoAmbientalFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ManejoAmbientalFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ManejoAmbientalObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ManejoAnticipoRutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ManejoTransitoFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ManejoTransitoFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ManejoTransitoObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitudModificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlanAprovechamientoForestal).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanInventarioArboreo).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanManejoAguasLluvias).HasComment("1=no,2=si,2=noSeRequiere");

                entity.Property(e => e.PlanRutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramaSaludFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ProgramaSaludFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ProgramaSaludObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramaSeguridadFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ProgramaSeguridadFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ProgramaSeguridadObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ResiduosDemolicionFechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.ResiduosDemolicionFechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.ResiduosDemolicionObservaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaInforme)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Utilidad).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotalFaseConstruccion).HasColumnType("numeric(18, 2)");

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones).IsRequired();

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoObservacion)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoObservacion_Contrato");
            });

            modelBuilder.Entity<ContratoPerfil>(entity =>
            {
                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.PerfilCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroRadicado).HasMaxLength(50);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoPerfil)
                    .WithMany(p => p.ContratoPerfilNumeroRadicado)
                    .HasForeignKey(d => d.ContratoPerfilId)
                    .HasConstraintName("FK_ContratoPerfilNumeroRadicado_ContratoPerfil");
            });

            modelBuilder.Entity<ContratoPerfilObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).HasMaxLength(3250);

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoPerfil)
                    .WithMany(p => p.ContratoPerfilObservacion)
                    .HasForeignKey(d => d.ContratoPerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPerfilObservacion_ContratoPerfil");
            });

            modelBuilder.Entity<ContratoPoliza>(entity =>
            {
                entity.Property(e => e.DescripcionModificacion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoPolizaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaExpedicion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreAseguradora)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCertificado)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroPoliza)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesRevisionGeneral)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsableAprobacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoModificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(400);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(400);

                entity.Property(e => e.ValorAmparo).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Vigencia).HasColumnType("datetime");

                entity.Property(e => e.VigenciaAmparo).HasColumnType("datetime");

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ContratoPoliza)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratoPoliza_Contrato");
            });

            modelBuilder.Entity<ControlRecurso>(entity =>
            {
                entity.Property(e => e.FechaConsignacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorConsignacion).HasColumnType("numeric(18, 2)");

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
                entity.Property(e => e.ActuacionAdelantadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ActuacionAdelantadaOtro)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActuacionReclamacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoAvanceTramiteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacionOtro)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ResumenPropuestaFiduciaria)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaContractual)
                    .WithMany(p => p.ControversiaActuacion)
                    .HasForeignKey(d => d.ControversiaContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaActuacion_ControversiaContractual");
            });

            modelBuilder.Entity<ControversiaActuacionMesa>(entity =>
            {
                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoAvanceMesaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacionAdelantada).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacionRequerida)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.ControversiaActuacionMesa)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Controver__Contr__23DE44F1");
            });

            modelBuilder.Entity<ControversiaActuacionMesaSeguimiento>(entity =>
            {
                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoAvanceMesaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacionAdelantada).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.NumeroActuacionSeguimiento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacionRequerida)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaActuacionMesa)
                    .WithMany(p => p.ControversiaActuacionMesaSeguimiento)
                    .HasForeignKey(d => d.ControversiaActuacionMesaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Controver__Contr__2F4FF79D");
            });

            modelBuilder.Entity<ControversiaContractual>(entity =>
            {
                entity.Property(e => e.ConclusionComitePreTecnico)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaComitePreTecnico).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.MotivoJustificacionRechazo)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoSac)
                    .HasColumnName("NumeroRadicadoSAC")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoControversiaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contrato)
                    .WithMany(p => p.ControversiaContractual)
                    .HasForeignKey(d => d.ContratoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaContractual_Contrato");
            });

            modelBuilder.Entity<ControversiaMotivo>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.MotivoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaContractual)
                    .WithMany(p => p.ControversiaMotivo)
                    .HasForeignKey(d => d.ControversiaContractualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControversiaMotivo_ControversiaContractual");
            });

            modelBuilder.Entity<CronogramaSeguimiento>(entity =>
            {
                entity.Property(e => e.EstadoActividadFinalCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActividadInicialCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccionCronograma)
                    .WithMany(p => p.CronogramaSeguimiento)
                    .HasForeignKey(d => d.ProcesoSeleccionCronogramaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CronogramaSeguimiento_ProcesoSeleccionCronograma");
            });

            modelBuilder.Entity<CuentaBancaria>(entity =>
            {
                entity.Property(e => e.BancoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreCuentaBanco)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuentaBanco)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuentaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.CuentaBancaria)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_CuentaBancaria_FuenteFinanciacion");
            });

            modelBuilder.Entity<DefensaJudicial>(entity =>
            {
                entity.Property(e => e.CanalIngresoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CuantiaPerjuicios).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

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

                entity.Property(e => e.Pretensiones)
                    .HasMaxLength(1000)
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

            modelBuilder.Entity<DefensaJudicialContratacionProyecto>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.ActuacionAdelantada)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoProcesoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ProximaActuacion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.DefensaJudicialSeguimiento)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefensaJudicialSeguimiento_DefensaJudicial");
            });

            modelBuilder.Entity<DemandadoConvocado>(entity =>
            {
                entity.Property(e => e.CaducidadPrescripcion).HasColumnType("date");

                entity.Property(e => e.ConvocadoAutoridadDespacho)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaProcesoFfiecodigo)
                    .HasColumnName("EtapaProcesoFFIECodigo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRadicado).HasColumnType("datetime");

                entity.Property(e => e.MedioControlAccion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RadicadoDespacho)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.DemandadoConvocado)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .HasConstraintName("FK_Demandado_DefensaJucicial");
            });

            modelBuilder.Entity<DemandanteConvocante>(entity =>
            {
                entity.HasKey(e => e.DemandanteConvocadoId);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefensaJucicial)
                    .WithMany(p => p.DemandanteConvocante)
                    .HasForeignKey(d => d.DefensaJucicialId)
                    .HasConstraintName("FK_Demandante_DefensaJucicial");
            });

            modelBuilder.Entity<DisponibilidadPresupuestal>(entity =>
            {
                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaDdp)
                    .HasColumnName("FechaDDP")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaDrp)
                    .HasColumnName("FechaDRP")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.LimitacionEspecial)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDrp)
                    .HasColumnName("NumeroDRP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroRadicadoSolicitud)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.OpcionContratarCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RutaDdp)
                    .HasColumnName("RutaDDP")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudEspecialCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporte).HasMaxLength(1000);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAportante).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorSolicitud).HasColumnType("numeric(18, 2)");

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
                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.DisponibilidadPresupuestal)
                    .WithMany(p => p.DisponibilidadPresupuestalObservacion)
                    .HasForeignKey(d => d.DisponibilidadPresupuestalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisponibilidadPresupuestalObservacion_DisponibilidadPresupuestal");
            });

            modelBuilder.Entity<DisponibilidadPresupuestalProyecto>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroDocumento)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDocumentoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.VigenciaAporteCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.DocumentoApropiacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoApropiacion_Aportante");
            });

            modelBuilder.Entity<Dominio>(entity =>
            {
                entity.Property(e => e.DominioId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica que la parametrica esta activa en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Código de la parametrica en el sistema si lo tiene");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Descripción de la  parametrica en el sistema");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación de la parametrica");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria (TipoDominioId, Codigo, Nombre, Descripcion, Activo)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del Tipo de parametrica en el sistema");

                entity.Property(e => e.TipoDominioId).HasComment("Identificador de la tabla del Tipo de dominio al que pertenece la parametrica");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Email del Usuario que crea la nueva parametrica");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");

                entity.HasOne(d => d.TipoDominio)
                    .WithMany(p => p.Dominio)
                    .HasForeignKey(d => d.TipoDominioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dominio_TipoDominio");
            });

            modelBuilder.Entity<EnsayoLaboratorioMuestra>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEntregaResultado).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreMuestra).HasMaxLength(40);

                entity.Property(e => e.Observacion).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.ComponenteId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FaseId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UsoId)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FichaEstudio>(entity =>
            {
                entity.Property(e => e.Abogado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AnalisisJuridico)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Antecedentes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DecisionComiteDirectrices)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EsPresentadoAnteComiteFfie).HasColumnName("EsPresentadoAnteComiteFFIE");

                entity.Property(e => e.FechaComiteDefensa).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.HechosRelevantes)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.JurisprudenciaDoctrina)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.RecomendacionFinalComite)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Recomendaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.TipoActuacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefensaJudicial)
                    .WithMany(p => p.FichaEstudio)
                    .HasForeignKey(d => d.DefensaJudicialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FichaEstudio_DefensaJudicial");
            });

            modelBuilder.Entity<FlujoInversion>(entity =>
            {
                entity.Property(e => e.Semana)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

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

            modelBuilder.Entity<FuenteFinanciacion>(entity =>
            {
                entity.HasIndex(e => new { e.AportanteId, e.Eliminado })
                    .HasName("indexaportante");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FuenteRecursosCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFuente).HasColumnType("numeric(18, 2)");

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
                entity.Property(e => e.DisponibilidadPresupuestalId).HasColumnName("DisponibilidadPresupuestalID");

                entity.Property(e => e.DisponibilidadPresupuestalProyectoId).HasColumnName("DisponibilidadPresupuestalProyectoID");

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NuevoSaldo).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SaldoActual).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorSolicitado).HasColumnType("numeric(18, 2)");

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
            });

            modelBuilder.Entity<GestionObraCalidadEnsayoLaboratorio>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEntregaResultados).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTomaMuestras).HasColumnType("datetime");

                entity.Property(e => e.TipoEnsayoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.GrupoMunicipiosId).ValueGeneratedNever();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccionGrupo)
                    .WithMany(p => p.GrupoMunicipios)
                    .HasForeignKey(d => d.ProcesoSeleccionGrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GrupoMunicipios_ProcesoSeleccionGrupo");
            });

            modelBuilder.Entity<InfraestructuraIntervenirProyecto>(entity =>
            {
                entity.HasKey(e => e.InfraestrucutraIntervenirProyectoId);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEliminacion).HasColumnType("datetime");

                entity.Property(e => e.InfraestructuraCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioEliminacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.InfraestructuraIntervenirProyecto)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InfraestructuraIntervenirProyecto_Proyecto");
            });

            modelBuilder.Entity<InstitucionEducativaSede>(entity =>
            {
                entity.Property(e => e.CodigoDane)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Localizacion>(entity =>
            {
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
                    .HasComment("Identificador LocalizacionId Padre al que pertenece");

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Proveedor).HasMaxLength(100);

                entity.Property(e => e.UrlRegistroFotografico)
                    .HasMaxLength(500)
                    .IsFixedLength();

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ManejoMaterialesInsumos)
                    .WithMany(p => p.ManejoMaterialesInsumosProveedor)
                    .HasForeignKey(d => d.ManejoMaterialesInsumosId)
                    .HasConstraintName("fk_ManejoMaterialesInsumosProveedor_ManejoMaterialesInsumosId_1");
            });

            modelBuilder.Entity<ManejoOtro>(entity =>
            {
                entity.Property(e => e.FechaActividad).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstaCuantificadoRcd).HasColumnName("EstaCuantificadoRCD");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreGestorResiduos).HasMaxLength(255);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ManejoResiduosConstruccionDemolicion)
                    .WithMany(p => p.ManejoResiduosConstruccionDemolicionGestor)
                    .HasForeignKey(d => d.ManejoResiduosConstruccionDemolicionId)
                    .HasConstraintName("fk_ManejoResiduosConstruccionDemolicionGestor_ManejoResiduosConstruccionDemolicion_1");
            });

            modelBuilder.Entity<ManejoResiduosPeligrososEspeciales>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UrlRegistroFotografico).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.MensajesValidacionesId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica que la parametrica esta activa en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Código del mensaje");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación de la parametrica");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria (MensajesValidacionesId, Codigo, Mensaje, Modulo, Activo)");

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasComment("Mensaje Validacion");

                entity.Property(e => e.MenuId).HasComment("Modulo al que pertenecen las validaciones");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Email del Usuario que crea la nueva parametrica");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MensajesValidaciones)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("Fk_MensajesValidaciones_MenuId_Fk_Menu_MenuId");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Descripción del Menu en el sistema");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el menú fue eliminado (0)Menú vigente (1)MNenú Eliminado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del Menú");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria ( Nombre,Descripción,Posición,Icono,RutaFormulario)");

                entity.Property(e => e.Icono)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Icono");

                entity.Property(e => e.MenuPadreId).HasComment("Identificador del Menu Padre");

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
                    .HasComment("Email del Usuario que crea al nuevo menú");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");
            });

            modelBuilder.Entity<MenuPerfil>(entity =>
            {
                entity.Property(e => e.MenuPerfilId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica si el registro esta activo (0) Inactivo (1) Activo");

                entity.Property(e => e.Crud)
                    .HasColumnName("CRUD")
                    .HasComment("Indica si el perfil tiene permisos de CRUD en la funcionalidad");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de Creación del registro");

                entity.Property(e => e.MenuId).HasComment("Identificador del Menú");

                entity.Property(e => e.PerfilId).HasComment("Identificador del PerfilId");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Nombre de usuario que creo el registro");

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
                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.MesEjecucion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MesEjecucion_ContratoConstruccion");
            });

            modelBuilder.Entity<NovedadContractual>(entity =>
            {
                entity.Property(e => e.AjusteClausula)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ClausulaModificar)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ConceptoTecnico)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.EsAplicadaAcontrato).HasColumnName("EsAplicadaAContrato");

                entity.Property(e => e.FechaConcepto).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaFinSuspension).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioSuspension).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSesionInstancia).HasColumnType("datetime");

                entity.Property(e => e.FechaSolictud).HasColumnType("datetime");

                entity.Property(e => e.InstanciaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MotivoNovedadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PresupuestoAdicionalSolicitado).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ResumenJustificacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoNovedadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(400);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(400);
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.Property(e => e.PerfilId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el usuario fue eliminado (0)Usuario vigente (1)Usuario Eliminado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del Usuario");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria ( Email, contraseña, IsActivo,Observaciones)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del perfil de usuario en el sistema");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Email del Usuario que crea al nuevo usuario");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");
            });

            modelBuilder.Entity<Plantilla>(entity =>
            {
                entity.Property(e => e.Codigo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Encabezado)
                    .WithMany(p => p.InverseEncabezado)
                    .HasForeignKey(d => d.EncabezadoId)
                    .HasConstraintName("fk_EncabezadoId_Plantilla");

                entity.HasOne(d => d.PieDePagina)
                    .WithMany(p => p.InversePieDePagina)
                    .HasForeignKey(d => d.PieDePaginaId)
                    .HasConstraintName("fk_PiePagina_Plantilla");
            });

            modelBuilder.Entity<PolizaGarantia>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoGarantiaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(400);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(400);

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaGarantia)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolizaGarantia_ContratoPoliza");
            });

            modelBuilder.Entity<PolizaObservacion>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoRevisionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRevision).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(400);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(400);

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaObservacion)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolizaObservacion_ContratoPoliza");
            });

            modelBuilder.Entity<Predio>(entity =>
            {
                entity.Property(e => e.CedulaCatastral)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoAcreditacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPredioCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionLatitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionLongitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.InstitucionEducativaSede)
                    .WithMany(p => p.Predio)
                    .HasForeignKey(d => d.InstitucionEducativaSedeId)
                    .HasConstraintName("FK_Predio_InstitucionEducativaSede");
            });

            modelBuilder.Entity<ProcesoSeleccion>(entity =>
            {
                entity.Property(e => e.AlcanceParticular)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesAsignacionPuntaje)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesFinancierasHabilitantes)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesJuridicasHabilitantes)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesTecnicasHabilitantes)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CriteriosSeleccion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EvaluacionDescripcion)
                    .HasMaxLength(4000)
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

            modelBuilder.Entity<ProcesoSeleccionCotizacion>(entity =>
            {
                entity.HasIndex(e => new { e.ProcesoSeleccionId, e.NombreOrganizacion, e.ValorCotizacion, e.UrlSoporte, e.Eliminado })
                    .HasName("UK_ProcesoSeleccion")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreOrganizacion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSoporte)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorCotizacion).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionCotizacion)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionCotizacion_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionCronograma>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaActualProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaMaxima).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionCronograma)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionCronograma_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionCronogramaMonitoreo>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaActualProcesoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaMaxima).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccionMonitoreo)
                    .WithMany(p => p.ProcesoSeleccionCronogramaMonitoreo)
                    .HasForeignKey(d => d.ProcesoSeleccionMonitoreoId)
                    .HasConstraintName("fk_ProcesoSeleccionCronogramaMonitoreo_ProcesoSeleccionMonitoreo");
            });

            modelBuilder.Entity<ProcesoSeleccionGrupo>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreGrupo)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPresupuestoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorMaximoCategoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorMinimoCategoria).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionGrupo)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionGrupo_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionIntegrante>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreIntegrante)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionIntegrante)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionIntegrante_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionMonitoreo>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActividadCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroProceso)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionMonitoreo)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoMonitoreo_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionObservacion)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionObservacion_ProcesoSeleccion");
            });

            modelBuilder.Entity<ProcesoSeleccionProponente>(entity =>
            {
                entity.Property(e => e.CedulaRepresentanteLegal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionProponente)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EmailProponente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProponente)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NombreRepresentanteLegal)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoProponente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIdentificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProponenteCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionProponente)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionProponente_ProcesoSeleccion");
            });

            modelBuilder.Entity<Programacion>(entity =>
            {
                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.AvanceFisicoCapitulo).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.ProgramacionCapitulo).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.Programacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Programacion_ContratoConstruccion");
            });

            modelBuilder.Entity<ProgramacionPersonalContrato>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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
                entity.HasIndex(e => e.LlaveMen)
                    .HasName("uk_llavemen")
                    .IsUnique();

                entity.Property(e => e.CoordinacionResponsableCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProgramacionCodigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProyectoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreado).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProyectoAdministrativoAportante>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEdicion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioEdicion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProyectoAdminstrativo)
                    .WithMany(p => p.ProyectoAdministrativoAportante)
                    .HasForeignKey(d => d.ProyectoAdminstrativoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoAportante_ProtectoAportanteId");
            });

            modelBuilder.Entity<ProyectoAportante>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorInterventoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorObra).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotalAportante).HasColumnType("numeric(18, 2)");

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

            modelBuilder.Entity<ProyectoFuentes>(entity =>
            {
                entity.HasKey(e => e.ProyectoFuenteId)
                    .HasName("PK_ProyectoAportante_copy1");

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.DireccionUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoHojasDeVidaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoRequisitoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PerfilCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.ProyectoRequisitoTecnico)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProyectoRequisitoTecnico_Proyecto");
            });

            modelBuilder.Entity<RegistroPresupuestal>(entity =>
            {
                entity.HasIndex(e => new { e.AportanteId, e.NumeroRp, e.FechaRp, e.CofinanciacionDocumentoId })
                    .HasName("UK_RP")
                    .IsUnique();

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRp)
                    .HasColumnName("FechaRP")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumeroRp)
                    .HasColumnName("NumeroRP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.RegistroPresupuestal)
                    .HasForeignKey(d => d.AportanteId)
                    .HasConstraintName("FK_RegistroPresupuestal_Aportante");

                entity.HasOne(d => d.CofinanciacionDocumento)
                    .WithMany(p => p.RegistroPresupuestal)
                    .HasForeignKey(d => d.CofinanciacionDocumentoId)
                    .HasConstraintName("FK_RegistroPresupuestalDocumento");
            });

            modelBuilder.Entity<RequisitoTecnicoRadicado>(entity =>
            {
                entity.HasKey(e => e.RequisitoTecnicoRadicado1);

                entity.Property(e => e.RequisitoTecnicoRadicado1).HasColumnName("RequisitoTecnicoRadicado");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroRadicadoFfie)
                    .IsRequired()
                    .HasColumnName("NumeroRadicadoFFIE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProyectoRequisitoTecnico)
                    .WithMany(p => p.RequisitoTecnicoRadicado)
                    .HasForeignKey(d => d.ProyectoRequisitoTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisitoTecnicoRadicado_ProyectoRequisitoTecnico");
            });

            modelBuilder.Entity<SeguimientoActuacionDerivada>(entity =>
            {
                entity.Property(e => e.DescripciondeActuacionAdelantada)
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActuacionDerivadaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActuacionDerivada).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ControversiaActuacion)
                    .WithMany(p => p.SeguimientoActuacionDerivada)
                    .HasForeignKey(d => d.ControversiaActuacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoActuacionDerivada_ControversiaActuacion");
            });

            modelBuilder.Entity<SeguimientoDiario>(entity =>
            {
                entity.Property(e => e.CausaIndisponibilidadEquipoCodigo).HasMaxLength(2);

                entity.Property(e => e.CausaIndisponibilidadMaterialCodigo).HasMaxLength(2);

                entity.Property(e => e.CausaIndisponibilidadProductividadCodigo).HasMaxLength(2);

                entity.Property(e => e.DisponibilidadEquipoCodigo).HasMaxLength(2);

                entity.Property(e => e.DisponibilidadMaterialCodigo).HasMaxLength(2);

                entity.Property(e => e.EstadoCodigo).HasMaxLength(2);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSeguimiento).HasColumnType("date");

                entity.Property(e => e.FechaValidacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVerificacion).HasColumnType("datetime");

                entity.Property(e => e.ProductividadCodigo).HasMaxLength(2);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SeguimientoDiario)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoDiario_ContratacionProyecto");

                entity.HasOne(d => d.ObservacionSupervisor)
                    .WithMany(p => p.SeguimientoDiario)
                    .HasForeignKey(d => d.ObservacionSupervisorId)
                    .HasConstraintName("FK_SeguimientoDiario_SeguimientoDiarioObservaciones");

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoDiario)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoDiario_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoDiarioObservaciones>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoDiarioNavigation)
                    .WithMany(p => p.SeguimientoDiarioObservaciones)
                    .HasForeignKey(d => d.SeguimientoDiarioId)
                    .HasConstraintName("FK_SeguimientoDiarioObservaciones_SeguimientoDiario");
            });

            modelBuilder.Entity<SeguimientoSemanal>(entity =>
            {
                entity.Property(e => e.EstadoMuestrasCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioSupervisor).HasColumnType("datetime");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.SeguimientoSemanal)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanal_ContratacionProyecto");
            });

            modelBuilder.Entity<SeguimientoSemanalAvanceFinanciero>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.AvanceFisicoSemanal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.EstadoObraCodigo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.ProgramacionSemanal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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

            modelBuilder.Entity<SeguimientoSemanalGestionObra>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalGestionObra)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalGestionObra_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalGestionObraAlerta>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TemaCapacitacion).HasMaxLength(300);

                entity.Property(e => e.UrlSoporteGestion).HasMaxLength(255);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UrlSoporteGestion).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(50);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(50);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoObservacionCodigo)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalObservacion)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalObervavion_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalPersonalObra>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoSemanal)
                    .WithMany(p => p.SeguimientoSemanalPersonalObra)
                    .HasForeignKey(d => d.SeguimientoSemanalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeguimientoSemanalPersonalObra_SeguimientoSemanal");
            });

            modelBuilder.Entity<SeguimientoSemanalRegistrarComiteObra>(entity =>
            {
                entity.Property(e => e.FechaComite).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroComite).HasMaxLength(255);

                entity.Property(e => e.UrlSoporteComite).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UrlSoporteFotografico).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

            modelBuilder.Entity<SeguridadSaludCausaAccidente>(entity =>
            {
                entity.HasKey(e => e.SeguridadSaludCausaAccidentesId)
                    .HasName("PK__Segurida__60218A2A407CA8DC");

                entity.Property(e => e.CausaAccidenteCodigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoSemanalGestionObraSeguridadSalud)
                    .WithMany(p => p.SeguridadSaludCausaAccidente)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraSeguridadSaludId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SeguridadSaludCausaAccidente_SeguimientoSemanalGestionObraSeguridadSalud_1");
            });

            modelBuilder.Entity<SesionComentario>(entity =>
            {
                entity.Property(e => e.EstadoActaVoto)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.DesarrolloSolicitud)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.DesarrolloSolicitudFiduciario)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActaCodigo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActaCodigoFiduciario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaComiteFiduciario).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesFiduciario)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporteVotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporteVotacionFiduciario)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioComiteFiduciario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Responsable)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Tarea)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteTecnicoCompromiso)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTecnicoCompromiso_ComiteTecnico");
            });

            modelBuilder.Entity<SesionComiteTema>(entity =>
            {
                entity.HasKey(e => e.SesionTemaId);

                entity.Property(e => e.EstadoTemaCodigo).HasMaxLength(255);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesDecision)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsableCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporte)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Tema)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteTema)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .HasConstraintName("FK_SesionComiteTema_ComiteTecnico");
            });

            modelBuilder.Entity<SesionInvitado>(entity =>
            {
                entity.Property(e => e.Cargo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Entidad)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionInvitado)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .HasConstraintName("FK_SesionInvitado_ComiteTecnico");
            });

            modelBuilder.Entity<SesionParticipante>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("date");

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesDevolucion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Tarea)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion).IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioDocumentacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaTramite).HasColumnType("datetime");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaMinuta)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
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

            modelBuilder.Entity<TemaCompromiso>(entity =>
            {
                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Tarea)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
                entity.Property(e => e.EstadoCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Tarea).HasMaxLength(500);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.EstadoObra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContrato)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempFlujoInversion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Semana)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempFlujoInversion)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempFlujo__Archi__17236851");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.TempFlujoInversion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempFlujo__Contr__18178C8A");
            });

            modelBuilder.Entity<TempOrdenLegibilidad>(entity =>
            {
                entity.Property(e => e.CcrlutoConsorcio).HasColumnName("CCRLUToConsorcio");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoRl)
                    .IsRequired()
                    .HasColumnName("CorreoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("CorreoRLUToConsorcio")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DepartamentoRl).HasColumnName("DepartamentoRL");

                entity.Property(e => e.DepartamentoRlutoConsorcio).HasColumnName("DepartamentoRLUToConsorcio");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionRl)
                    .IsRequired()
                    .HasColumnName("DireccionRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("DireccionRLUToConsorcio")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Legal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MinicipioRlutoConsorcio).HasColumnName("MinicipioRLUToConsorcio");

                entity.Property(e => e.MunucipioRl).HasColumnName("MunucipioRL");

                entity.Property(e => e.NombreEntidad)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NombreIntegrante)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreOtoConsorcio)
                    .IsRequired()
                    .HasColumnName("NombreOToConsorcio")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreRlutoConsorcio)
                    .IsRequired()
                    .HasColumnName("NombreRLUToConsorcio")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIddentificacionProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeParticipacion).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RepresentanteLegal)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoRl)
                    .IsRequired()
                    .HasColumnName("TelefonoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoRlutoConsorcio)
                    .HasColumnName("TelefonoRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempOrdenLegibilidad)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempOrden__Archi__3EA749C6");
            });

            modelBuilder.Entity<TempProgramacion>(entity =>
            {
                entity.Property(e => e.Actividad)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TempProgramacion)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempProgr__Archi__116A8EFB");

                entity.HasOne(d => d.ContratoConstruccion)
                    .WithMany(p => p.TempProgramacion)
                    .HasForeignKey(d => d.ContratoConstruccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TempProgr__Contr__125EB334");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.Property(e => e.Contenido).IsRequired();

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);
            });

            modelBuilder.Entity<TemporalProyecto>(entity =>
            {
                entity.Property(e => e.Aportante1).HasColumnName("Aportante_1");

                entity.Property(e => e.Aportante2).HasColumnName("Aportante_2");

                entity.Property(e => e.Aportante3).HasColumnName("Aportante_3");

                entity.Property(e => e.CedulaCatastralPredio).HasMaxLength(20);

                entity.Property(e => e.CodigoDaneIe).HasColumnName("CodigoDaneIE");

                entity.Property(e => e.DireccionPredioPrincipal).HasMaxLength(20);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSesionJunta).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .IsRequired()
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(8);

                entity.Property(e => e.NumeroDocumentoAcreditacion).HasMaxLength(20);

                entity.Property(e => e.TipoAportanteId1).HasColumnName("TipoAportanteId_1");

                entity.Property(e => e.TipoAportanteId2).HasColumnName("TipoAportanteId_2");

                entity.Property(e => e.TipoAportanteId3).HasColumnName("TipoAportanteId_3");

                entity.Property(e => e.UbicacionPredioPrincipalLatitud)
                    .IsRequired()
                    .HasColumnName("UbicacionPredioPrincipal_Latitud")
                    .HasMaxLength(10);

                entity.Property(e => e.UbicacionPredioPrincipalLontitud)
                    .IsRequired()
                    .HasColumnName("UbicacionPredioPrincipal_Lontitud")
                    .HasMaxLength(10);

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);

                entity.Property(e => e.ValorInterventoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorObra).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotal).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ArchivoCargue)
                    .WithMany(p => p.TemporalProyecto)
                    .HasForeignKey(d => d.ArchivoCargueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_archivoCargeId_ArchivoCarge_archivoCargeId");
            });

            modelBuilder.Entity<TipoActividadGestionObra>(entity =>
            {
                entity.Property(e => e.TipoActividadCodigo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SeguimientoSemanalGestionObra)
                    .WithMany(p => p.TipoActividadGestionObra)
                    .HasForeignKey(d => d.SeguimientoSemanalGestionObraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoActividadGestionObra_SeguimientoSemanalGestionObra");
            });

            modelBuilder.Entity<TipoDominio>(entity =>
            {
                entity.Property(e => e.TipoDominioId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Activo).HasComment("Indica que el tipo de parametrica esta activo en el sistema (0)Desactivado (1)Vigente");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Descripción del Tipo de parametrica en el sistema");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del tipo de parametrica");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria ( Nombre, Descripcion, Activo)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del Tipo de parametrica en el sistema");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Email del Usuario que crea al nuevo tipo de parametrica");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.UsuarioId).HasComment("Identificador de la tabla");

                entity.Property(e => e.Activo)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Indica si el usuario se encuentra activo en el sistema");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Bloqueado).HasComment("Indica si el usuario se encuentra bloqueado por seguridad y numero de intentos fallidos en el sistema");

                entity.Property(e => e.CambiarContrasena).HasDefaultValueSql("('0')");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("Contraseña del Usuario, campo cifrado");

                entity.Property(e => e.Eliminado)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Indica que el usuario fue eliminado (0)Usuario vigente (1)Usuario Eliminado");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Identificación de usuario definido por correo electrónico");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasComment("Fecha de creación del Usuario");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasComment("Si el registro se actualiza con respecto a los campos que no son de auditoria ( Email, contraseña, IsActivo,Observaciones)");

                entity.Property(e => e.FechaUltimoIngreso)
                    .HasColumnType("datetime")
                    .HasComment("Fecha que se registra y actualiza apenas ingresa el usuario al sistema.");

                entity.Property(e => e.IntentosFallidos).HasComment("Cantidad de intentos de ingreso fallidos por contraseña.");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Dirección Ip del dispositivo o equipo de conexión del usuario");

                entity.Property(e => e.IpProxy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Ip proxy de la conexión del usuario");

                entity.Property(e => e.NombreMaquina)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Nombre del equipo o dispositivo desde donde se esta conectando el usuario por ultima vez.");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Se incluyen algunas observaciones si las hay al momento de CRUD del usuario.");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Email del Usuario que crea al nuevo usuario");

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Usuario que realizo la modificación de los datos no de auditoria");
            });

            modelBuilder.Entity<UsuarioPerfil>(entity =>
            {
                entity.HasNoKey();

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

                entity.HasOne(d => d.Perfil)
                    .WithMany()
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPerfil");

                entity.HasOne(d => d.Usuario)
                    .WithMany()
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_usuario");
            });

            modelBuilder.Entity<VGestionarGarantiasPolizas>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GestionarGarantiasPolizas");

                entity.Property(e => e.EstadoPoliza)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoPolizaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacionContrato).HasColumnType("datetime");

                entity.Property(e => e.FechaFirma).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
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

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigoContratacion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudContratacion)
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

                entity.Property(e => e.EstadoJuridicoPredios)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoProyecto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VProyectosXcontrato>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ProyectosXContrato");

                entity.Property(e => e.Departamento)
                    .HasColumnName("departamento")
                    .HasMaxLength(300)
                    .IsUnicode(false);

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

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasColumnName("sede")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .IsRequired()
                    .HasColumnName("tipoIntervencion")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VRegistrarAvanceSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_RegistrarAvanceSemanal");

                entity.Property(e => e.EstadoObra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContrato)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActaInicioFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionApoyo).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoNombreVerificacion)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActaInicioFase1).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaAprobacionRequisitosConstruccionInterventor).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaFase1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VValidarSeguimientoSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_ValidarSeguimientoSemanal");

                entity.Property(e => e.EstadoMuestras)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoObra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaModificacionAvalar).HasColumnType("datetime");

                entity.Property(e => e.FechaReporte).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VVerificarSeguimientoSemanal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_VerificarSeguimientoSemanal");

                entity.Property(e => e.EstadoMuestras)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoObra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSeguimientoSemanalCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaModificacionVerificar).HasColumnType("datetime");

                entity.Property(e => e.FechaReporte).HasColumnType("datetime");

                entity.Property(e => e.InstitucionEducativa)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LlaveMen)
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sede)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VigenciaAporte>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.TipoVigenciaCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAporte).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.VigenciaAporte)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_VigenciaAporte_FuenteFinanciacion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
