using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class FlujoInversion
    {
        public int FlujoInversionId { get; set; }
        public int ConstruccionCargueId { get; set; }
        public string Capitulo { get; set; }
        public int? Mes { get; set; }
        public decimal? Valor { get; set; }

        public virtual ConstruccionCargue ConstruccionCargue { get; set; }
    }
}
