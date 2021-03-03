using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProgramacionBySeguimientoSemanal
    {
        public int SeguimientoSemanalId { get; set; }
        public int ProgramacionId { get; set; }
        public string Actividad { get; set; }
        public string TipoActividadCodigo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; }
    }
}
