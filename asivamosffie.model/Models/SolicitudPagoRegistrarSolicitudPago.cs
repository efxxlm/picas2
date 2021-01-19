using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoRegistrarSolicitudPago
    {
        public int SolicitudPagoRegistrarSolicitudPagoId { get; set; }
        public int SolicitudPagoId { get; set; }
        public bool? TieneFasePrecosntruccion { get; set; }
        public bool? TieneFaseConstruccion { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroRadicadoSac { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
