using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyectoAportanteHistorico
    {
        public int ContratacionProyectoAportanteHistoricoId { get; set; }
        public int ContratacionProyectoAportanteId { get; set; }
        public decimal ValorAporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ContratacionProyectoAportante ContratacionProyectoAportante { get; set; }
    }
}
