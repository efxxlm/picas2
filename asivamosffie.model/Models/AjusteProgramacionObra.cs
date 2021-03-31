using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AjusteProgramacionObra
    {
        public int AjusteProgramacionObraId { get; set; }
        public int AjusteProgramacionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string TipoActividadCodigo { get; set; }
        public string Actividad { get; set; }
        public bool EsRutaCritica { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; }

        public virtual AjusteProgramacion AjusteProgramacion { get; set; }
    }
}
