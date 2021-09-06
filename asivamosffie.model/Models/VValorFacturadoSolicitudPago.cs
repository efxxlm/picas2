using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoSolicitudPago
    {
        public int? ContratoId { get; set; }
        public decimal? Valor { get; set; }
        public int SolicitudPagoId { get; set; }
    }
}
