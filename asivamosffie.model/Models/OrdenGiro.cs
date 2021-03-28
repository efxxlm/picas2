using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiro
    {
        public OrdenGiro()
        {
            BalanceFinancieroTranslado = new HashSet<BalanceFinancieroTranslado>();
            OrdenGiroObservacion = new HashSet<OrdenGiroObservacion>();
            OrdenGiroTerceroNavigation = new HashSet<OrdenGiroTercero>();
            SolicitudPago = new HashSet<SolicitudPago>();
        }

        public int OrdenGiroId { get; set; }
        public int? OrdenGiroTerceroId { get; set; }
        public int? OrdenGiroDetalleId { get; set; }
        public int? OrdenGiroObservacionId { get; set; }
        public int? OrdenGiroSoporteId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }
        public string NumeroSolicitud { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
        public virtual OrdenGiroTercero OrdenGiroTercero { get; set; }
        public virtual ICollection<BalanceFinancieroTranslado> BalanceFinancieroTranslado { get; set; }
        public virtual ICollection<OrdenGiroObservacion> OrdenGiroObservacion { get; set; }
        public virtual ICollection<OrdenGiroTercero> OrdenGiroTerceroNavigation { get; set; }
        public virtual ICollection<SolicitudPago> SolicitudPago { get; set; }
    }
}
