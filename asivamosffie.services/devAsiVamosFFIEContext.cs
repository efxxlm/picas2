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

        public virtual DbSet<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual DbSet<ArchivoCargue> ArchivoCargue { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<Cofinanciacion> Cofinanciacion { get; set; }
        public virtual DbSet<CofinanciacionAportante> CofinanciacionAportante { get; set; }
        public virtual DbSet<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
        public virtual DbSet<ComiteTecnico> ComiteTecnico { get; set; }
        public virtual DbSet<ComiteTecnicoProyecto> ComiteTecnicoProyecto { get; set; }
        public virtual DbSet<ComponenteAportante> ComponenteAportante { get; set; }
        public virtual DbSet<ComponenteUso> ComponenteUso { get; set; }
        public virtual DbSet<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
        public virtual DbSet<Contratacion> Contratacion { get; set; }
        public virtual DbSet<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual DbSet<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual DbSet<Contratista> Contratista { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<ContratoPoliza> ContratoPoliza { get; set; }
        public virtual DbSet<ControlRecurso> ControlRecurso { get; set; }
        public virtual DbSet<CronogramaSeguimiento> CronogramaSeguimiento { get; set; }
        public virtual DbSet<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual DbSet<DisponibilidadPresupuestal> DisponibilidadPresupuestal { get; set; }
        public virtual DbSet<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
        public virtual DbSet<Dominio> Dominio { get; set; }
        public virtual DbSet<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual DbSet<GestionFuenteFinanciacion> GestionFuenteFinanciacion { get; set; }
        public virtual DbSet<GrupoMunicipios> GrupoMunicipios { get; set; }
        public virtual DbSet<InfraestructuraIntervenirProyecto> InfraestructuraIntervenirProyecto { get; set; }
        public virtual DbSet<InstitucionEducativaSede> InstitucionEducativaSede { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<MensajesValidaciones> MensajesValidaciones { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuPerfil> MenuPerfil { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PolizaGarantia> PolizaGarantia { get; set; }
        public virtual DbSet<PolizaObservacion> PolizaObservacion { get; set; }
        public virtual DbSet<Predio> Predio { get; set; }
        public virtual DbSet<ProcesoSeleccion> ProcesoSeleccion { get; set; }
        public virtual DbSet<ProcesoSeleccionCotizacion> ProcesoSeleccionCotizacion { get; set; }
        public virtual DbSet<ProcesoSeleccionCronograma> ProcesoSeleccionCronograma { get; set; }
        public virtual DbSet<ProcesoSeleccionGrupo> ProcesoSeleccionGrupo { get; set; }
        public virtual DbSet<ProcesoSeleccionIntegrante> ProcesoSeleccionIntegrante { get; set; }
        public virtual DbSet<ProcesoSeleccionObservacion> ProcesoSeleccionObservacion { get; set; }
        public virtual DbSet<ProcesoSeleccionProponente> ProcesoSeleccionProponente { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<ProyectoAdministrativo> ProyectoAdministrativo { get; set; }
        public virtual DbSet<ProyectoAdministrativoAportante> ProyectoAdministrativoAportante { get; set; }
        public virtual DbSet<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual DbSet<ProyectoPredio> ProyectoPredio { get; set; }
        public virtual DbSet<ProyectoRequisitoTecnico> ProyectoRequisitoTecnico { get; set; }
        public virtual DbSet<RegistroPresupuestal> RegistroPresupuestal { get; set; }
        public virtual DbSet<RequisitoTecnicoRadicado> RequisitoTecnicoRadicado { get; set; }
        public virtual DbSet<Sesion> Sesion { get; set; }
        public virtual DbSet<SesionComentario> SesionComentario { get; set; }
        public virtual DbSet<SesionComiteInvitadoVoto> SesionComiteInvitadoVoto { get; set; }
        public virtual DbSet<SesionComiteTecnico> SesionComiteTecnico { get; set; }
        public virtual DbSet<SesionComiteTecnicoCompromiso> SesionComiteTecnicoCompromiso { get; set; }
        public virtual DbSet<SesionComiteTema> SesionComiteTema { get; set; }
        public virtual DbSet<SesionInvitado> SesionInvitado { get; set; }
        public virtual DbSet<TemaCompromiso> TemaCompromiso { get; set; }
        public virtual DbSet<TempOrdenLegibilidad> TempOrdenLegibilidad { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemporalProyecto> TemporalProyecto { get; set; }
        public virtual DbSet<TipoDominio> TipoDominio { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<VigenciaAporte> VigenciaAporte { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=asivamosffie.database.windows.net;Database=devAsiVamosFFIE;User ID=adminffie;Password=SaraLiam2020*;MultipleActiveResultSets=False;Connection Timeout=30;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.AportanteFuenteFinanciacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aportante_AportanteId_2");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.AportanteFuenteFinanciacion)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FuenteFinanciacion_FuenteFinanciacionId");
            });

            modelBuilder.Entity<ArchivoCargue>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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

            modelBuilder.Entity<Cofinanciacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);
            });

            modelBuilder.Entity<CofinanciacionAportante>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion).HasMaxLength(200);

                entity.Property(e => e.UsuarioModificacion).HasMaxLength(200);

                entity.HasOne(d => d.Cofinanciacion)
                    .WithMany(p => p.CofinanciacionAportante)
                    .HasForeignKey(d => d.CofinanciacionId)
                    .HasConstraintName("fk_CofinanciacionAportante_Cofinanciacion_1");
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
                entity.Property(e => e.EstadoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratacionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
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

                entity.HasOne(d => d.Contratista)
                    .WithMany(p => p.ComiteTecnico)
                    .HasForeignKey(d => d.ContratistaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComiteTecnico_Contratista");
            });

            modelBuilder.Entity<ComiteTecnicoProyecto>(entity =>
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

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.ComiteTecnicoProyecto)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComiteTecnicoProyecto_ComiteTecnico");

                entity.HasOne(d => d.ContratacionProyecto)
                    .WithMany(p => p.ComiteTecnicoProyecto)
                    .HasForeignKey(d => d.ContratacionProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComiteTecnicoProyecto_ContratacionProyectoId");
            });

            modelBuilder.Entity<ComponenteAportante>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

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

                entity.Property(e => e.ValorUso).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.ComponenteAportante)
                    .WithMany(p => p.ComponenteUso)
                    .HasForeignKey(d => d.ComponenteAportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponenteAportanteId_ComponenteAportante");
            });

            modelBuilder.Entity<CompromisoSeguimiento>(entity =>
            {
                entity.Property(e => e.DescripcionSeguimiento)
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Miembro)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SesionComiteTecnicoCompromiso)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.SesionComiteTecnicoCompromisoId)
                    .HasConstraintName("FK_CompromisoSeguimiento_SesionComiteTecnicoCompromiso");

                entity.HasOne(d => d.TemaCompromiso)
                    .WithMany(p => p.CompromisoSeguimiento)
                    .HasForeignKey(d => d.TemaCompromisoId)
                    .HasConstraintName("FK_CompromisoSeguimiento_TemaCompromiso");
            });

            modelBuilder.Entity<Contratacion>(entity =>
            {
                entity.Property(e => e.ConsideracionDescripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaEnvioDocumentacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NumeroSolicitud)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaMinuta)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
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
                    .HasConstraintName("FK_ContratistaId_Contratista");
            });

            modelBuilder.Entity<ContratacionProyecto>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaVigencia).HasColumnType("datetime");

                entity.Property(e => e.NumeroLicencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeAvanceObra).HasColumnType("numeric(3, 2)");

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
                    .HasConstraintName("FK_ContratacionId_Contratacion");

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

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.ContratacionProyectoAportante)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContratacionProyectoAportante_Aportante");

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

                entity.Property(e => e.RepresentanteLegal)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIdentificacionCodigo)
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

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.Property(e => e.EstadoDocumentoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEnvioFirma).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContratista).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaContrato).HasColumnType("datetime");

                entity.Property(e => e.FechaFirmaFiduciaria).HasColumnType("datetime");

                entity.Property(e => e.FechaTramite).HasColumnType("datetime");

                entity.Property(e => e.NumeroContrato)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Plazo).HasColumnType("datetime");

                entity.Property(e => e.RutaDocumento)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContratoCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Contratacion)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.ContratacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contrato_Contratacion");
            });

            modelBuilder.Entity<ContratoPoliza>(entity =>
            {
                entity.Property(e => e.ContratoPolizaId).ValueGeneratedNever();

                entity.Property(e => e.DescripcionModificacion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");

                entity.Property(e => e.FechaExpedicion).HasColumnType("datetime");

                entity.Property(e => e.NombreAseguradora)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCertificado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroPoliza)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesRevisionGeneral)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsableAprobacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoModificacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoSolicitudCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAmparo).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Vigencia).HasColumnType("datetime");

                entity.Property(e => e.VigenciaAmparo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

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
                    .HasConstraintName("FK_ControlRecurso_CuentaBancaria");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .HasConstraintName("FK_ControlRecurso_FuenteFinanciacion");

                entity.HasOne(d => d.RegistroPresupuestal)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.RegistroPresupuestalId)
                    .HasConstraintName("FK_ControlRecurso_RegistroPresupuestal");

                entity.HasOne(d => d.VigenciaAporte)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.VigenciaAporteId)
                    .HasConstraintName("FK_ControlRecurso_VigenciaAporte");
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

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
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

            modelBuilder.Entity<DisponibilidadPresupuestal>(entity =>
            {
                entity.Property(e => e.EstadoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaDdp)
                    .HasColumnName("FechaDDP")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NumeroDdp)
                    .HasColumnName("NumeroDDP")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.OpcionContratarCodigo)
                    .IsRequired()
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

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorSolicitud).HasColumnType("numeric(18, 2)");
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

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.DisponibilidadPresupuestalProyecto)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisponibilidadPresupuestalProyecto_Proyecto");
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

            modelBuilder.Entity<FuenteFinanciacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FuenteRecursosCodigo)
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

                entity.Property(e => e.ValorFuente).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.FuenteFinanciacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FuenteFinanciacion_Aportante");
            });

            modelBuilder.Entity<GestionFuenteFinanciacion>(entity =>
            {
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

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.GestionFuenteFinanciacion)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GestionFuenteFinanciacion_FuenteFinanciacion");
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

                entity.Property(e => e.CoordinacionResponsableCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
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
                    .HasMaxLength(50)
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

            modelBuilder.Entity<PolizaGarantia>(entity =>
            {
                entity.Property(e => e.PolizaGarantiaId).ValueGeneratedNever();

                entity.Property(e => e.TipoGarantiaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContratoPoliza)
                    .WithMany(p => p.PolizaGarantia)
                    .HasForeignKey(d => d.ContratoPolizaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PolizaGarantia_ContratoPoliza");
            });

            modelBuilder.Entity<PolizaObservacion>(entity =>
            {
                entity.Property(e => e.EstadoRevisionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRevision).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

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
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesAsignacionPuntaje)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesFinancierasHabilitantes)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesJuridicasHabilitantes)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.CondicionesTecnicasHabilitantes)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.CriteriosSeleccion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EtapaProcesoSeleccionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EvaluacionDescripcion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroProceso)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .HasMaxLength(2000)
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

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProcesoSeleccionCotizacion>(entity =>
            {
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
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActividadCodigo)
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

            modelBuilder.Entity<ProcesoSeleccionGrupo>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreGrupo)
                    .HasMaxLength(500)
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

            modelBuilder.Entity<ProcesoSeleccionObservacion>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
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

                entity.HasOne(d => d.ProcesoSeleccion)
                    .WithMany(p => p.ProcesoSeleccionProponente)
                    .HasForeignKey(d => d.ProcesoSeleccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesoSeleccionProponente_ProcesoSeleccion");
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoJuridicoCodigo)
                    .HasMaxLength(100)
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

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.ProyectoAdministrativoAportante)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aportante_AportanteId");

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

            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.Property(e => e.EstadoComiteCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaOrdenDia).HasColumnType("datetime");

                entity.Property(e => e.NumeroComite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaActaSesion)
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

            modelBuilder.Entity<SesionComentario>(entity =>
            {
                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Miembro)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

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

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.SesionComentario)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComentario_Sesion");
            });

            modelBuilder.Entity<SesionComiteInvitadoVoto>(entity =>
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

                entity.HasOne(d => d.SesionComiteTecnico)
                    .WithMany(p => p.SesionComiteInvitadoVoto)
                    .HasForeignKey(d => d.SesionComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteInvitadoVoto_SesionComiteTecnico");

                entity.HasOne(d => d.SesionInvitado)
                    .WithMany(p => p.SesionComiteInvitadoVoto)
                    .HasForeignKey(d => d.SesionInvitadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteInvitadoVoto_SesionInvitado");
            });

            modelBuilder.Entity<SesionComiteTecnico>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaSoporteVotacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ComiteTecnico)
                    .WithMany(p => p.SesionComiteTecnico)
                    .HasForeignKey(d => d.ComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTecnico_ComiteTecnico");

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.SesionComiteTecnico)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTecnico_Sesion");
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

                entity.HasOne(d => d.SesionComiteTecnico)
                    .WithMany(p => p.SesionComiteTecnicoCompromiso)
                    .HasForeignKey(d => d.SesionComiteTecnicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTecnicoCompromiso_SesionComiteTecnico");
            });

            modelBuilder.Entity<SesionComiteTema>(entity =>
            {
                entity.HasKey(e => e.SesionTemaId);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ObservacionesDecision)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsableCodigo)
                    .IsRequired()
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

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.SesionComiteTema)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionComiteTema_Sesion");
            });

            modelBuilder.Entity<SesionInvitado>(entity =>
            {
                entity.Property(e => e.Cargo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Entidad)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.SesionInvitado)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SesionInvitado_Sesion");
            });

            modelBuilder.Entity<TemaCompromiso>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCumplimiento).HasColumnType("datetime");

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

                entity.HasOne(d => d.SesionTema)
                    .WithMany(p => p.TemaCompromiso)
                    .HasForeignKey(d => d.SesionTemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemaCompromiso_SesionComiteTema");
            });

            modelBuilder.Entity<TempOrdenLegibilidad>(entity =>
            {
                entity.HasKey(e => e.TempId)
                    .HasName("PK__TempOrde__17892B5A4ADD7748");

                entity.Property(e => e.TempId)
                    .HasColumnName("Temp_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CcrlutoConsorcio)
                    .HasColumnName("CCRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CedulaRepresentanteLegal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoRl)
                    .HasColumnName("CorreoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoRlutoConsorcio)
                    .HasColumnName("CorreoRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Departamento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DepartamentoRl)
                    .HasColumnName("DepartamentoRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DepartamentoRlutoConsorcio)
                    .HasColumnName("DepartamentoRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionRl)
                    .HasColumnName("DireccionRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionRlutoConsorcio)
                    .HasColumnName("DireccionRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EntiddaesQueIntegranLaUnionTemporal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.IdentificacionTributaria)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Legal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Minicipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MinicipioRlutoConsorcio)
                    .HasColumnName("MinicipioRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MunucipioRl)
                    .HasColumnName("MunucipioRL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEntidad)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreIntegrante)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreOtoConsorcio)
                    .HasColumnName("NombreOToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreRlutoConsorcio)
                    .HasColumnName("NombreRLUToConsorcio")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroIddentificacionProponente)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeParticipacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RepresentanteLegal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoRl)
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
