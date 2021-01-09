using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaContractual
    {
        public ControversiaContractual()
        {
            ControversiaActuacion = new HashSet<ControversiaActuacion>();
            ControversiaActuacionMesa = new HashSet<ControversiaActuacionMesa>();
            ControversiaMotivo = new HashSet<ControversiaMotivo>();
        }

        public int ControversiaContractualId { get; set; }
        public string TipoControversiaCodigo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoCodigo { get; set; }
        public bool EsCompleto { get; set; }
        public int SolicitudId { get; set; }
        public int ContratoId { get; set; }
        public DateTime? FechaComitePreTecnico { get; set; }
        public string ConclusionComitePreTecnico { get; set; }
        public bool? EsProcede { get; set; }
        public string NumeroRadicadoSac { get; set; }
        public string MotivoJustificacionRechazo { get; set; }
        public bool? EsRequiereComite { get; set; }
        public string RutaSoporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<ControversiaActuacion> ControversiaActuacion { get; set; }
        public virtual ICollection<ControversiaActuacionMesa> ControversiaActuacionMesa { get; set; }
        public virtual ICollection<ControversiaMotivo> ControversiaMotivo { get; set; }
    }
}
