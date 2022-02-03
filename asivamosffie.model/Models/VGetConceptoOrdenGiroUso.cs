using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VGetConceptoOrdenGiroUso
    {
        public bool? EstaAprobada { get; set; }
        public int ContratoId { get; set; }
        public int? ContratacionId { get; set; }
        public int? ProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionAportanteId { get; set; }
        public int OrdenGiroId { get; set; }
        public int? AportanteId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal? Descuentos { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public string ConceptoPago { get; set; }
        public string Uso { get; set; }
        public string UsoCodigo { get; set; }
        public string TipoPago { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public decimal? ValorAmortizacion { get; set; }
        public bool EsFactura { get; set; }
    }
}
