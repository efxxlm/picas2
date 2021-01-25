using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SeguimientoActuacionDerivada
    {
        public int SeguimientoActuacionDerivadaId { get; set; }
        public int ControversiaActuacionId { get; set; }
        public bool? EsRequiereFiduciaria { get; set; }
        public DateTime? FechaActuacionDerivada { get; set; }
        public string DescripciondeActuacionAdelantada { get; set; }
        public string RutaSoporte { get; set; }
        public string EstadoActuacionDerivadaCodigo { get; set; }
        public string Observaciones { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ControversiaActuacion ControversiaActuacion { get; set; }
    }
}
