using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FlujoInversion
    {
        public int FlujoInversionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string Capitulo { get; set; }
        public int? Mes { get; set; }
        public decimal? Valor { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
    }
}
