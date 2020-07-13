using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyectoAportante
    {
        public ContratacionProyectoAportante()
        {
            ComponenteAportante = new HashSet<ComponenteAportante>();
        }

        public int ContratacionProyectoAportanteId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int AportanteId { get; set; }
        public decimal ValorAporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Aportante Aportante { get; set; }
        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<ComponenteAportante> ComponenteAportante { get; set; }
    }
}
