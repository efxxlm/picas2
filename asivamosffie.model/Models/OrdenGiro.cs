using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiro
    {
        public int OrdenGiroId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public int? OrdenGiroTerceroId { get; set; }
        public int? OrdenGiroDetalleId { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
        public virtual OrdenGiroTercero OrdenGiroTercero { get; set; }
        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
