using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class Programacion
    {
        public Programacion()
        {
            FlujoInversion = new HashSet<FlujoInversion>();
        }

        public int ProgramacionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string TipoActividadCodigo { get; set; }
        public string Actividad { get; set; }
        public bool EsRutaCritica { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; }
        public decimal? AvanceFisicoCapitulo { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
        public virtual ICollection<FlujoInversion> FlujoInversion { get; set; }
    }
}
