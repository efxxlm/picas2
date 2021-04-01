using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldoPresupuestalXcontrato
    {
        public int? ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public decimal? ValorDdpobraInterventoria { get; set; }
        public decimal? ValorFacturadoObraInterventoria { get; set; }
        public decimal? SaldoPresupuestalObraInterventoria { get; set; }
        public decimal? ValorDdpotrosCostos { get; set; }
        public decimal? ValorFacturadoOtrosCostos { get; set; }
        public decimal? SaldoPresupuestalOtrosCostos { get; set; }
    }
}
