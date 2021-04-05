using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
            ContratoApoyo = new HashSet<Contrato>();
            ContratoInterventor = new HashSet<Contrato>();
            ContratoPolizaActualizacionRevisionAprobacionObservacion = new HashSet<ContratoPolizaActualizacionRevisionAprobacionObservacion>();
            ContratoSupervisor = new HashSet<Contrato>();
            NovedadContractual = new HashSet<NovedadContractual>();
            SesionComentario = new HashSet<SesionComentario>();
            SesionParticipante = new HashSet<SesionParticipante>();
            UsuarioPerfil = new HashSet<UsuarioPerfil>();
        }

        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public bool? Activo { get; set; }
        public bool? Bloqueado { get; set; }
        public int? IntentosFallidos { get; set; }
        public bool? Eliminado { get; set; }
        public string NombreMaquina { get; set; }
        public string Ip { get; set; }
        public string IpProxy { get; set; }
        public DateTime? FechaUltimoIngreso { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? CambiarContrasena { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string MunicipioId { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public string UrlSoporteDocumentacion { get; set; }
        public string Observaciones { get; set; }
        public string DependenciaCodigo { get; set; }
        public string GrupoCodigo { get; set; }
        public DateTime? FechaCambioPassword { get; set; }
        public string ProcedenciaCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string TipoAsignacionCodigo { get; set; }
        public bool? TieneContratoAsignado { get; set; }
        public bool? TieneGrupo { get; set; }

        public virtual Localizacion Municipio { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
        public virtual ICollection<Contrato> ContratoApoyo { get; set; }
        public virtual ICollection<Contrato> ContratoInterventor { get; set; }
        public virtual ICollection<ContratoPolizaActualizacionRevisionAprobacionObservacion> ContratoPolizaActualizacionRevisionAprobacionObservacion { get; set; }
        public virtual ICollection<Contrato> ContratoSupervisor { get; set; }
        public virtual ICollection<NovedadContractual> NovedadContractual { get; set; }
        public virtual ICollection<SesionComentario> SesionComentario { get; set; }
        public virtual ICollection<SesionParticipante> SesionParticipante { get; set; }
        public virtual ICollection<UsuarioPerfil> UsuarioPerfil { get; set; }
    }
}
