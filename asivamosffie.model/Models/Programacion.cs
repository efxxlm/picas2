using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Programacion
    {
        public int ProgramacionId { get; set; }
        public int ConstruccionCargueId { get; set; }
        public string TipoActividadCodigo { get; set; }
        public string Actividad { get; set; }
        public bool EsRutaCritica { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; }

        public virtual ConstruccionCargue ConstruccionCargue { get; set; }
    }
}
