using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoOtrosCostosServicios
    {
        public int SolicitudPagoOtrosCostosServiciosId { get; set; }
        public int SolicitudPagoId { get; set; }
        public int? NumeroRadicadoSac { get; set; }
        public string NumeroFactura { get; set; }
        public string ValorFacturado { get; set; }
        public string TipoPagoCodigo { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
