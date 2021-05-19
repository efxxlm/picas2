using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinanciero
    {
        public BalanceFinanciero()
        {
            BalanceFinancieroTraslado = new HashSet<BalanceFinancieroTraslado>();
        }

        public int BalanceFinancieroId { get; set; }
        public int ProyectoId { get; set; }
        public bool? RequiereTransladoRecursos { get; set; }
        public string EstadoBalanceCodigo { get; set; }
        public string JustificacionTrasladoAportanteFuente { get; set; }
        public string UrlSoporte { get; set; }
        public bool? EstaAnulado { get; set; }
        public DateTime? FechaAnulado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroBalance { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<BalanceFinancieroTraslado> BalanceFinancieroTraslado { get; set; }
    }
}
