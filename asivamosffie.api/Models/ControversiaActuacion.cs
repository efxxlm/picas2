using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ControversiaActuacion
    {
        public ControversiaActuacion()
        {
            ActuacionSeguimiento = new HashSet<ActuacionSeguimiento>();
        }

        public int ControversiaActuacionId { get; set; }
        public int ControversiaContractualId { get; set; }
        public DateTime? FechaActuacion { get; set; }
        public string ActuacionAdelantadaCodigo { get; set; }
        public string ActuacionAdelantadaOtro { get; set; }
        public string ProximaActuacionCodigo { get; set; }
        public string ProximaActuacionOtro { get; set; }
        public int? CantDiasVencimiento { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool? EsRequiereContratista { get; set; }
        public bool? EsRequiereInterventor { get; set; }
        public bool? EsRequiereSupervisor { get; set; }
        public bool? EsRequiereJuridico { get; set; }
        public bool? EsRequiereFiduciaria { get; set; }
        public bool? EsRequiereComite { get; set; }
        public string Observaciones { get; set; }
        public bool? EsRequiereAseguradora { get; set; }
        public string ResumenPropuestaFiduciaria { get; set; }
        public bool? EsRequiereComiteReclamacion { get; set; }
        public bool? EsprocesoResultadoDefinitivo { get; set; }
        public string RutaSoporte { get; set; }
        public string EstadoAvanceTramiteCodigo { get; set; }
        public bool? EsRequiereMesaTrabajo { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string EstadoActuacionReclamacionCodigo { get; set; }

        public virtual ControversiaContractual ControversiaContractual { get; set; }
        public virtual ICollection<ActuacionSeguimiento> ActuacionSeguimiento { get; set; }
    }
}
