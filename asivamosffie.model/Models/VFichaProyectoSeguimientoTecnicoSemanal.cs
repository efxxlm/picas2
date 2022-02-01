using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoSeguimientoTecnicoSemanal
    {
        public int ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public int SeguimientoSemanalAvanceFisicoId { get; set; }
        public int NumeroSemana { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string EstadoObra { get; set; }
        public decimal? ProgramacionObra { get; set; }
        public decimal? AvanceFisico { get; set; }
    }
}
