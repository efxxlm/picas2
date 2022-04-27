using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratoProyectoFechaEstimadaFinalizacion
    {
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public DateTime? FechaInicioProyecto { get; set; }
        public DateTime? FechaEstimadaFinProyecto { get; set; }
        public DateTime? FechaFinProyecto { get; set; }
        public DateTime? FechaFinContrato { get; set; }
        public DateTime? FechaEstimadaFinContrato { get; set; }
        public int? SemanasProyecto { get; set; }
        public int? SemanasEstimadasProyecto { get; set; }
        public int? PlazoDiasContrato { get; set; }
        public int? PlazoMesesContrato { get; set; }
        public int? PlazoDiasProyecto { get; set; }
        public int? PlazoMesesProyecto { get; set; }
        public int? PlazoEstimadoMesesProyecto { get; set; }
        public int? PlazoEstimadoDiasProyecto { get; set; }
        public int? PlazoEstimadoMesesContrato { get; set; }
        public int? PlazoEstimadoDiasContrato { get; set; }
    }
}
