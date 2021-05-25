using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VEjecucionFinancieraXproyecto
    {
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public decimal? TotalComprometido { get; set; }
        public decimal OrdenadoGirarAntesImpuestos { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? PorcentajeEjecucionFinanciera { get; set; }
    }
}
