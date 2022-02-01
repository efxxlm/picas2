using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOdgValoresFacturados
    {
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public int SolicitudPagoId { get; set; }
        public int OrdenGiroId { get; set; }
        public int ProyectoId { get; set; }
        public int? AportanteId { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public string UsoCodigo { get; set; }
        public string NombreUso { get; set; }
        public decimal? ValorDescuento { get; set; }
    }
}
