﻿using System;
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
        public virtual DbSet<ComponenteAportante> ComponenteAportante { get; set; }
        public virtual DbSet<ComponenteUso> ComponenteUso { get; set; }
        public virtual DbSet<Contratacion> Contratacion { get; set; }
        public virtual DbSet<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual DbSet<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual DbSet<Contratista> Contratista { get; set; }
        public virtual DbSet<ControlRecurso> ControlRecurso { get; set; }
        public virtual DbSet<CronogramaSeguimiento> CronogramaSeguimiento { get; set; }
        public virtual DbSet<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual DbSet<Dominio> Dominio { get; set; }
        public virtual DbSet<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual DbSet<GrupoMunicipios> GrupoMunicipios { get; set; }
        public virtual DbSet<InfraestructuraIntervenirProyecto> InfraestructuraIntervenirProyecto { get; set; }
        public virtual DbSet<InstitucionEducativaSede> InstitucionEducativaSede { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<MensajesValidaciones> MensajesValidaciones { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuPerfil> MenuPerfil { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
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
        public virtual DbSet<RegistroPresupuestal> RegistroPresupuestal { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemporalProyecto> TemporalProyecto { get; set; }
        public virtual DbSet<TipoDominio> TipoDominio { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public virtual DbSet<VigenciaAporte> VigenciaAporte { get; set; }



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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_CofinanciacionAportanteId_FK_CofinanciacionAportante_CofinanciacionAportanteId");
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

            modelBuilder.Entity<Contratacion>(entity =>
            {
                entity.Property(e => e.ConsideracionDescripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoSolicitudCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");

                entity.Property(e => e.NumeroSolicitud)
                    .IsRequired()
                    .HasMaxLength(10)
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

            modelBuilder.Entity<ControlRecurso>(entity =>
            {
                entity.Property(e => e.FechaConsignacion).HasColumnType("datetime");

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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ControlRecurso_RegistroPresupuestal");

                entity.HasOne(d => d.VigenciaAporte)
                    .WithMany(p => p.ControlRecurso)
                    .HasForeignKey(d => d.VigenciaAporteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoSifi)
                    .HasColumnName("CodigoSIFI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NombreCuentaBanco)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroCuentaBanco)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCuentaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.CuentaBancaria)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CuentaBancaria_FuenteFinanciacion");
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

                entity.Property(e => e.FuenteRecursosCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorFuente).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.FuenteFinanciacion)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FuenteFinanciacion_Aportante");
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
                    .HasMaxLength(15)
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

            modelBuilder.Entity<Predio>(entity =>
            {
                entity.Property(e => e.CedulaCatastral)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoAcreditacionCodigo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPredioCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionLatitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionLongitud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
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
                    .IsRequired()
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
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroProceso)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Objeto)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.TipoAlcanceCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencionCodigo)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreOrganizacion)
                    .IsRequired()
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
                entity.Property(e => e.ProcesoSeleccionCronogramaId).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstadoActividadCodigo)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPresupuestoCodigo)
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.FechaSesionJunta).HasColumnType("datetime");

                entity.Property(e => e.LlaveMen)
                    .IsRequired()
                    .HasColumnName("LlaveMEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LocalizacionIdMunicipio)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIntervencionCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPredioCodigo)
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

                entity.Property(e => e.ValorInterventoria).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorObra).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ValorTotal).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.InstitucionEducativa)
                    .WithMany(p => p.ProyectoInstitucionEducativa)
                    .HasForeignKey(d => d.InstitucionEducativaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proyecto_InstitucionEducativaSede");

                entity.HasOne(d => d.LocalizacionIdMunicipioNavigation)
                    .WithMany(p => p.Proyecto)
                    .HasForeignKey(d => d.LocalizacionIdMunicipio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proyecto_Localizacion");

                entity.HasOne(d => d.PredioPrincipal)
                    .WithMany(p => p.Proyecto)
                    .HasForeignKey(d => d.PredioPrincipalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proyecto_Predio");

                entity.HasOne(d => d.Sede)
                    .WithMany(p => p.ProyectoSede)
                    .HasForeignKey(d => d.SedeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
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

            modelBuilder.Entity<RegistroPresupuestal>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRp)
                    .HasColumnName("FechaRP")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumeroRp)
                    .IsRequired()
                    .HasColumnName("NumeroRP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Aportante)
                    .WithMany(p => p.RegistroPresupuestal)
                    .HasForeignKey(d => d.AportanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroPresupuestal_Aportante");
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
            });

            modelBuilder.Entity<VigenciaAporte>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.TipoVigenciaCodigo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAporte).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.FuenteFinanciacion)
                    .WithMany(p => p.VigenciaAporte)
                    .HasForeignKey(d => d.FuenteFinanciacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VigenciaAporte_FuenteFinanciacion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
