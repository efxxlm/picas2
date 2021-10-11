using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFacturadoXodgXcontratacionXproyectoXaportanteXfaseXconcepXuso
    {
        public int OrdenGiroId { get; set; }
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public string ConceptoCodigo { get; set; }
        public string UsoNombre { get; set; }
        public string UsoCodigo { get; set; }
        public int? AportanteId { get; set; }
        public decimal ValorUso { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal? SaldoUso { get; set; }
    }
}
