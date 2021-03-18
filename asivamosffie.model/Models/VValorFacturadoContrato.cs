using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoContrato
    {
        public int ContratoId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal? ValorSolicitudDdp { get; set; }
    }
}
