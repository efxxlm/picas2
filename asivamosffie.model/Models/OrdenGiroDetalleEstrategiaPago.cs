using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleEstrategiaPago
    {
        public OrdenGiroDetalleEstrategiaPago()
        {
            OrdenGiroDetalle = new HashSet<OrdenGiroDetalle>();
        }

        public int OrdenGiroDetalleEstrategiaPagoId { get; set; }
        public string EstrategiaPagoCodigo { get; set; }
        public bool RegistroCompleto { get; set; }

        public virtual ICollection<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
    }
}
