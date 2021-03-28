using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalle
    {
        public OrdenGiroDetalle()
        {
            OrdenGiroDetalleEstrategiaPago = new HashSet<OrdenGiroDetalleEstrategiaPago>();
            OrdenGiroDetalleTerceroCausacion = new HashSet<OrdenGiroDetalleTerceroCausacion>();
            OrdenGiroObservacion = new HashSet<OrdenGiroObservacion>();
            OrdenGiroSoporte = new HashSet<OrdenGiroSoporte>();
        }

        public int OrdenGiroDetalleId { get; set; }
        public int? OrdenGiroDetalleEstrategiaPagoId { get; set; }
        public int? OrdenGiroObservacionId { get; set; }
        public int? OrdenGiroSoporteId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroId { get; set; }

        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual ICollection<OrdenGiroDetalleEstrategiaPago> OrdenGiroDetalleEstrategiaPago { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacion> OrdenGiroDetalleTerceroCausacion { get; set; }
        public virtual ICollection<OrdenGiroObservacion> OrdenGiroObservacion { get; set; }
        public virtual ICollection<OrdenGiroSoporte> OrdenGiroSoporte { get; set; }
    }
}
