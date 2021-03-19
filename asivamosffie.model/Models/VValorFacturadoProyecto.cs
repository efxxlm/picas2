using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoProyecto
    {
        public int? ContratacionProyectoId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public bool? EsPreconstruccion { get; set; }
    }
}
