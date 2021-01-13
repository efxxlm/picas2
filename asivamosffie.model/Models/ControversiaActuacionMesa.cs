using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaActuacionMesa
    {
        public ControversiaActuacionMesa()
        {
            ControversiaActuacionMesaSeguimiento = new HashSet<ControversiaActuacionMesaSeguimiento>();
        }

        public int ControversiaActuacionMesaId { get; set; }
        public int ControversiaActuacionId { get; set; }
        public int ControversiaContractualId {get; set;}
        public ControversiaActuacionMesa()
        {
            ControversiaActuacionMesaSeguimiento = new HashSet<ControversiaActuacionMesaSeguimiento>();
        }

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

        public virtual ControversiaActuacion ControversiaActuacion { get; set; }
        public virtual ControversiaContractual ControversiaContractual { get; set; }
        public virtual ICollection<ControversiaActuacionMesaSeguimiento> ControversiaActuacionMesaSeguimiento { get; set; }
    }
}
