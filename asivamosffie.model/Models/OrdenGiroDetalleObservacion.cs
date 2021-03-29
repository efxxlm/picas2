using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleObservacion
    {
        public int OrdenGiroObservacionId { get; set; }
        public int OrdenGiroDetalleId { get; set; }
        public string Observacion { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
    }
}
