using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class MesEjecucion
    {
        public MesEjecucion()
        {
            FlujoInversion = new HashSet<FlujoInversion>();
        }

        public int MesEjecucionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public int Numero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
        public virtual ICollection<FlujoInversion> FlujoInversion { get; set; }
    }
}
