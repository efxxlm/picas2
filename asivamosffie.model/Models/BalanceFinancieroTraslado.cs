using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinancieroTraslado
    {
        public BalanceFinancieroTraslado()
        {
            BalanceFinancieroTrasladoValor = new HashSet<BalanceFinancieroTrasladoValor>();
        }

        public int BalanceFinancieroTrasladoId { get; set; }
        public int BalanceFinancieroId { get; set; }
        public int OrdenGiroId { get; set; }

        public virtual BalanceFinanciero BalanceFinanciero { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual ICollection<BalanceFinancieroTrasladoValor> BalanceFinancieroTrasladoValor { get; set; }
    }
}
