using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinanciero
    {
        public BalanceFinanciero()
        {
            BalanceFinancieroTranslado = new HashSet<BalanceFinancieroTranslado>();
        }

        public int BalanceFinancieroId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public bool? RequiereTransladoRecursos { get; set; }
        public string EstadoBalanceCodigo { get; set; }
        public string JustificacionTrasladoAportanteFuente { get; set; }
        public string UrlSoporte { get; set; }
        public bool? EstaAnulado { get; set; }
        public DateTime? FechaAnulado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroBalance { get; set; }
        public bool RegistroCompleto { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<BalanceFinancieroTranslado> BalanceFinancieroTranslado { get; set; }
    }
}
