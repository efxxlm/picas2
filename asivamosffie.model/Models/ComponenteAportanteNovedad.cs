using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteAportanteNovedad
    {
        public ComponenteAportanteNovedad()
        {
            ComponenteFuenteNovedad = new HashSet<ComponenteFuenteNovedad>();
        }

        public int ComponenteAportanteNovedadId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public string TipoComponenteCodigo { get; set; }
        public string FaseCodigo { get; set; }
        public bool? Eliminado { get; set; }
        public int? Activo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? NovedadContractualAportanteId { get; set; }

        public virtual CofinanciacionAportante CofinanciacionAportante { get; set; }
        public virtual NovedadContractualAportante NovedadContractualAportante { get; set; }
        public virtual ICollection<ComponenteFuenteNovedad> ComponenteFuenteNovedad { get; set; }
    }
}
