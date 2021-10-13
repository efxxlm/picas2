using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoAportanteHistorico
    {
        public int ProyectoAportanteHistoricoId { get; set; }
        public int ProyectoAportanteId { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotalAportante { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProyectoAportante ProyectoAportante { get; set; }
    }
}
