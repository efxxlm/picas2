using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAportantesXanticipoXcontrato
    {
        public int? ContratoId { get; set; }
        public decimal? ValorDescuento { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? AportanteId { get; set; }
    }
}
