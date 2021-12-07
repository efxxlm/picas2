using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class V_SeguimientoSemanalXSeguimientoSemanalAvanceFisicoProgramacion
    {
        public int? SeguimientoSemanalId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public decimal? AvanceFisicoSemanal { get; set; }
        public decimal? ProgramacionSemanal { get; set; }
        public string Actividad { get; set; }
    }
}
