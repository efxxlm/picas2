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
        public string NumeroTraslado { get; set; }
        public decimal? ValorTraslado { get; set; }
        public int? OrdenGiroId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string EstadoCodigo { get; set; }

        public virtual BalanceFinanciero BalanceFinanciero { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual ICollection<BalanceFinancieroTrasladoValor> BalanceFinancieroTrasladoValor { get; set; }
    }
}
