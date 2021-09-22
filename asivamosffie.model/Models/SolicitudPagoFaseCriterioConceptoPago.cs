using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseCriterioConceptoPago
    {
        public int SolicitudPagoFaseCriterioConceptoPagoId { get; set; }
        public int SolicitudPagoFaseCriterioId { get; set; }
        public string ConceptoPagoCriterio { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public decimal? ValorFacturadoConcepto { get; set; }
        public string UsoCodigo { get; set; }

        public virtual SolicitudPagoFaseCriterio SolicitudPagoFaseCriterio { get; set; }
    }
}
