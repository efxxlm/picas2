using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualAportante
    {
        public NovedadContractualAportante()
        {
            ComponenteAportanteNovedad = new HashSet<ComponenteAportanteNovedad>();
            NovedadContractualAportanteHistorico = new HashSet<NovedadContractualAportanteHistorico>();
        }

        public int NovedadContractualAportanteId { get; set; }
        public int? NovedadContractualId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public decimal? ValorAporte { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual CofinanciacionAportante CofinanciacionAportante { get; set; }
        public virtual NovedadContractual NovedadContractual { get; set; }
        public virtual ICollection<ComponenteAportanteNovedad> ComponenteAportanteNovedad { get; set; }
        public virtual ICollection<NovedadContractualAportanteHistorico> NovedadContractualAportanteHistorico { get; set; }
    }
}
