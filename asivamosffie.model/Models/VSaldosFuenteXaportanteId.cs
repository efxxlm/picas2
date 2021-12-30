using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldosFuenteXaportanteId
    {
        public int CofinanciacionAportanteId { get; set; }
        public decimal ComprometidoEnDdp { get; set; }
        public decimal RendimientosIncorporados { get; set; }
        public decimal? SaldoActual { get; set; }
        public int FuenteFinanciacionId { get; set; }
    }
}
