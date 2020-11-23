using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ActuacionSeguimiento
    {
        public int ActuacionSeguimientoId { get; set; }
        public int ControversiaActuacionId { get; set; }
        public bool SeguimientoCodigo { get; set; }
        public string EstadoReclamacionCodigo { get; set; }
        public DateTime? FechaActuacionAdelantada { get; set; }
        public string ActuacionAdelantada { get; set; }
        public string ProximaActuacion { get; set; }
        public int? CantDiasVencimiento { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Observaciones { get; set; }
        public string EstadoDerivadaCodigo { get; set; }
        public bool? EsResultadoDefinitivo { get; set; }
        public string RutaSoporte { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
