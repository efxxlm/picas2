using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyectoAportante
    {
        public ContratacionProyectoAportante()
        {
            ComponenteAportante = new HashSet<ComponenteAportante>();
            ContratacionProyectoAportanteHistorico = new HashSet<ContratacionProyectoAportanteHistorico>();
        }

        public int ContratacionProyectoAportanteId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal ValorAporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? CofinanciacionAportanteId { get; set; }

        public virtual CofinanciacionAportante CofinanciacionAportante { get; set; }
        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<ComponenteAportante> ComponenteAportante { get; set; }
        public virtual ICollection<ContratacionProyectoAportanteHistorico> ContratacionProyectoAportanteHistorico { get; set; }
    }
}
