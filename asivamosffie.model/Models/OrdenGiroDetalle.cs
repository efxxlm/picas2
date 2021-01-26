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

        public virtual OrdenGiroDetalleEstrategiaPago OrdenGiroDetalleEstrategiaPago { get; set; }
        public virtual ICollection<OrdenGiro> OrdenGiro { get; set; }
    }
}
