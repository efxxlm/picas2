using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroObservacion
    {
        public int OrdenGiroObservacionId { get; set; }
        public int OrdenGiroId { get; set; }
        public string Observacion { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public int? MenuId { get; set; }
        public bool? Archivada { get; set; }
        public int? IdPadre { get; set; }
        public bool? TieneObservacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
    }
}
