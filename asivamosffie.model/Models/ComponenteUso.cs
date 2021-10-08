using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteUso
    {
        public ComponenteUso()
        {
            ComponenteUsoHistorico = new HashSet<ComponenteUsoHistorico>();
        }

        public int ComponenteUsoId { get; set; }
        public int ComponenteAportanteId { get; set; }
        public string TipoUsoCodigo { get; set; }
        public decimal ValorUso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? Activo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? FuenteFinanciacionId { get; set; }

        public virtual ComponenteAportante ComponenteAportante { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual ICollection<ComponenteUsoHistorico> ComponenteUsoHistorico { get; set; }
    }
}
