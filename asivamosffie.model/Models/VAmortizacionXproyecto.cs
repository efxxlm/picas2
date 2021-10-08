using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAmortizacionXproyecto
    {
        public int SolicitudPagoId { get; set; }
        public int? ContratacionId { get; set; }
        public int? ContratoId { get; set; }
        public int? ProyectoId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? SolicitudPagoFaseCriterioConceptoPagoId { get; set; }
        public bool? TieneAnticipo { get; set; }
        public bool? ManejoAnticipoRequiere { get; set; }
        public string UsoCodigo { get; set; }
        public string UsoNombre { get; set; }
        public decimal? ValorAnticipo { get; set; }
        public decimal ValorAnticipoAmortizado { get; set; }
        public decimal? ValorPorAmortizar { get; set; }
    }
}
