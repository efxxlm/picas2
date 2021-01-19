using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoCargarFormaPago
    {
        public int SolicitudPagoCargarFormaPagoId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public string FaseConstruccionFormaPagoCodigo { get; set; }
        public string FasePreConstruccionFormaPagoCodigo { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
