using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldoPresupuestalXproyecto
    {
        public int? ProyectoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal? ValorDdp { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal? SaldoPresupuestal { get; set; }
    }
}
