using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoSoporteSolicitud
    {
        public int SolicitudPagoSoporteSolicitudId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public string UrlSoporte { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
