using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class PolizaObservacion
    {
        public int PolizaObservacionId { get; set; }
        public int ContratoPolizaId { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaRevision { get; set; }
        public string EstadoRevisionCodigo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
    }
}
