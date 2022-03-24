using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratoProyectoValorEstimado
    {
        public int ContratoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public decimal? ValorProyecto { get; set; }
        public decimal? ValorTotalProyecto { get; set; }
        public decimal? ValorContrato { get; set; }
        public decimal? ValorTotalContrato { get; set; }
    }
}
