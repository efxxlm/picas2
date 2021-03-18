using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratoPagosRealizados
    {
        public int SolicitudPagoId { get; set; }
        public int ContratoId { get; set; }
        public string FaseContrato { get; set; }
        public int? PagosRealizados { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string PorcentajeFacturado { get; set; }
        public decimal? SaldoPorPagar { get; set; }
        public string PorcentajePorPagar { get; set; }
    }
}
