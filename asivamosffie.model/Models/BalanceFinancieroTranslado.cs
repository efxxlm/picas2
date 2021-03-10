using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinancieroTranslado
    {
        public int BalanceFinancieroTransladoId { get; set; }
        public int BalanceFinancieroId { get; set; }
        public int OrdenGiroId { get; set; }

        public virtual BalanceFinanciero BalanceFinanciero { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
    }
}
