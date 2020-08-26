using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
        public int DefensaJudicialId { get; set; }
        public string LegitimacionCodigo { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public string NumeroProceso { get; set; }
        public int CantContratos { get; set; }
        public string EstadoProcesoCodigo { get; set; }
    }
}
