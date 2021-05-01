using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoXfasesSolicitudPago
    {
        public int SolicitudPagoId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public bool EsPreConstruccion { get; set; }
    }
}
