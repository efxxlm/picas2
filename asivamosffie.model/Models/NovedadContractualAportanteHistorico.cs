using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualAportanteHistorico
    {
        public int NovedadContractualAportanteHistoricoId { get; set; }
        public int? NovedadContractualAportanteId { get; set; }
        public decimal? ValorAporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual NovedadContractualAportante NovedadContractualAportante { get; set; }
    }
}
