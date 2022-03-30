using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratoProyectoFechaEstimadaFinalizacion
    {
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime? FechaInicioProyecto { get; set; }
        public DateTime? FechaEstimadaFinProyecto { get; set; }
        public DateTime? FechaFinProyecto { get; set; }
        public DateTime? FechaFinContrato { get; set; }
        public DateTime? FechaEstimadaFinContrato { get; set; }
        public int? SemanasProyecto { get; set; }
        public int? SemanasEstimadasProyecto { get; set; }
    }
}
