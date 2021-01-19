using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseDescuento
    {
        public int SolicitudPagoFaseDescuentoId { get; set; }
        public int SolicitudPagoFaseId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public decimal? ValorDescuento { get; set; }
    }
}
