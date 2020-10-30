using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPerfil
    {
        public ContratoPerfil()
        {
            ContratoPerfilNumeroRadicado = new HashSet<ContratoPerfilNumeroRadicado>();
            ContratoPerfilObservacion = new HashSet<ContratoPerfilObservacion>();
        }

        public int ContratoPerfilId { get; set; }
        public int ContratoId { get; set; }
        public string PerfilCodigo { get; set; }
        public int? CantidadHvRequeridas { get; set; }
        public int? CantidadHvRecibidas { get; set; }
        public int? CantidadHvAprobadas { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string RutaSoporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Eliminado { get; set; }
        public bool RegistroCompleto { get; set; }
        public int ProyectoId { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<ContratoPerfilNumeroRadicado> ContratoPerfilNumeroRadicado { get; set; }
        public virtual ICollection<ContratoPerfilObservacion> ContratoPerfilObservacion { get; set; }
    }
}
