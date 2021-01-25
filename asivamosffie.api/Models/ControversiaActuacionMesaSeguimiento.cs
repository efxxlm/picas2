using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ControversiaActuacionMesaSeguimiento
    {
        public int ControversiaActuacionMesaSeguimientoId { get; set; }
        public int ControversiaActuacionMesaId { get; set; }
        public string EstadoAvanceMesaCodigo { get; set; }
        public DateTime? FechaActuacionAdelantada { get; set; }
        public string ActuacionAdelantada { get; set; }
        public string ProximaActuacionRequerida { get; set; }
        public int? CantDiasVencimiento { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Observaciones { get; set; }
        public bool? ResultadoDefinitivo { get; set; }
        public string RutaSoporte { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string NumeroActuacionSeguimiento { get; set; }
        public string EstadoRegistroCodigo { get; set; }

        public virtual ControversiaActuacionMesa ControversiaActuacionMesa { get; set; }
    }
}
