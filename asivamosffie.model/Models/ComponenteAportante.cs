using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteAportante
    {
        public ComponenteAportante()
        {
            ComponenteUso = new HashSet<ComponenteUso>();
        }

        public int ComponenteAportanteId { get; set; }
        public int ContratacionProyectoAportanteId { get; set; }
        public string TipoComponenteCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ContratacionProyectoAportante ContratacionProyectoAportante { get; set; }
        public virtual ICollection<ComponenteUso> ComponenteUso { get; set; }
    }
}
