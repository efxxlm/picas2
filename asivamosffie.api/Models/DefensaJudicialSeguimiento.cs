using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class DefensaJudicialSeguimiento
    {
        public int DefensaJudicialSeguimientoId { get; set; }
        public int DefensaJudicialId { get; set; }
        public string EstadoProcesoCodigo { get; set; }
        public DateTime? FechaActuacion { get; set; }
        public string ActuacionAdelantada { get; set; }
        public string ProximaActuacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool? EsRequiereSupervisor { get; set; }
        public string Observaciones { get; set; }
        public bool? EsprocesoResultadoDefinitivo { get; set; }
        public string RutaSoporte { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual DefensaJudicial DefensaJudicial { get; set; }
    }
}
