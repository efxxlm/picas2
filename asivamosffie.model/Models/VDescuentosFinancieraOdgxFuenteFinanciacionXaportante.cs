using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDescuentosFinancieraOdgxFuenteFinanciacionXaportante
    {
        public int OrdenGiroId { get; set; }
        public int? AportanteId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public decimal? ValorDescuento { get; set; }
    }
}
