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

        public virtual DbSet<Dominio> Dominio { get; set; }
        public virtual DbSet<Localizacion> Localizacion { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuPerfil> MenuPerfil { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TipoDominio> TipoDominio { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=asivamosffie.database.windows.net;Database=devAsiVamosFFIE;User ID=adminffie;Password=SaraLiam2020*;MultipleActiveResultSets=False;Connection Timeout=30;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Bloqueado).HasComment("Indica si el usuario se encuentra bloqueado por seguridad y numero de intentos fallidos en el sistema");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
