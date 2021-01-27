using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalle
    {
        public OrdenGiroDetalle()
        {
            OrdenGiro = new HashSet<OrdenGiro>();
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
        public int? OrdenGiroDetalleDescuentoTecnicaId { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionId { get; set; }

        public virtual OrdenGiroDetalleDescuentoTecnica OrdenGiroDetalleDescuentoTecnica { get; set; }
        public virtual OrdenGiroDetalleEstrategiaPago OrdenGiroDetalleEstrategiaPago { get; set; }
        public virtual OrdenGiroDetalleTerceroCausacion OrdenGiroDetalleTerceroCausacion { get; set; }
        public virtual OrdenGiroObservacion OrdenGiroObservacion { get; set; }
        public virtual OrdenGiroSoporte OrdenGiroSoporte { get; set; }
        public virtual ICollection<OrdenGiro> OrdenGiro { get; set; }
    }
}
