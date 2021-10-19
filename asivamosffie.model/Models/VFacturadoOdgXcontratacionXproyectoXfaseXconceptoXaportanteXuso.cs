using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFacturadoOdgXcontratacionXproyectoXfaseXconceptoXaportanteXuso
    {
        public int? ContratoId { get; set; }
        public int? ContratacionId { get; set; }
        public int? ProyectoId { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public string ConceptoCodigo { get; set; }
        public int? AportanteId { get; set; }
        public decimal? ValorDescuento { get; set; }
        public string TipoUsoCodigo { get; set; }
    }
}
