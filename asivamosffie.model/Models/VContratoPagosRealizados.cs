using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratoPagosRealizados
    {
        public int ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public bool EsPreConstruccion { get; set; }
        public string FaseContrato { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal ValorDdpxFase { get; set; }
        public decimal? SaldoPorPagar { get; set; }
        public decimal? PorcentajeFacturado { get; set; }
        public decimal? PorcentajePorPagar { get; set; }
    }
}
