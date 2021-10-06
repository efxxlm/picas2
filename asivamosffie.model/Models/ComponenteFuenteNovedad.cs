using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteFuenteNovedad
    {
        public ComponenteFuenteNovedad()
        {
            ComponenteUsoNovedad = new HashSet<ComponenteUsoNovedad>();
            ComponenteUsoNovedadHistorico = new HashSet<ComponenteUsoNovedadHistorico>();
        }

        public int ComponenteFuenteNovedadId { get; set; }
        public int ComponenteAportanteNovedadId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FuenteRecursosCodigo { get; set; }

        public virtual ComponenteAportanteNovedad ComponenteAportanteNovedad { get; set; }
        public virtual ICollection<ComponenteUsoNovedad> ComponenteUsoNovedad { get; set; }
        public virtual ICollection<ComponenteUsoNovedadHistorico> ComponenteUsoNovedadHistorico { get; set; }
    }
}
