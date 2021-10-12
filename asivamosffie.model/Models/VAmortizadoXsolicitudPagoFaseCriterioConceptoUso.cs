using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAmortizadoXsolicitudPagoFaseCriterioConceptoUso
    {
        public int SolicitudPagoId { get; set; }
        public int? ContratoId { get; set; }
        public int SolicitudPagoFaseAmortizacionId { get; set; }
        public int? SolicitudPagoFaseCriterioConceptoPagoId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public decimal? ValorAmortizacion { get; set; }
    }
}
