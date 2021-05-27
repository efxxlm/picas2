using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VEjecucionPresupuestalXproyecto
    {
        public int? ProyectoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string Nombre { get; set; }
        public decimal? TotalComprometido { get; set; }
        public decimal FacturadoAntesImpuestos { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? PorcentajeEjecucionPresupuestal { get; set; }
    }
}
