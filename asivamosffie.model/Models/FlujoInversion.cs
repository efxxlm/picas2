using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FlujoInversion
    {
        public int FlujoInversionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string Semana { get; set; }
        public decimal? Valor { get; set; }
        public int? MesEjecucionId { get; set; }
        public int? ProgramacionId { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
        public virtual MesEjecucion MesEjecucion { get; set; }
        public virtual Programacion Programacion { get; set; }
    }
}
