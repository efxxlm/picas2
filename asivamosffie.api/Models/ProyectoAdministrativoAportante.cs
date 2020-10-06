using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ProyectoAdministrativoAportante
    {
        public ProyectoAdministrativoAportante()
        {
            AportanteFuenteFinanciacion = new HashSet<AportanteFuenteFinanciacion>();
        }

        public int ProyectoAdministrativoAportanteId { get; set; }
        public int ProyectoAdminstrativoId { get; set; }
        public int AportanteId { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioEdicion { get; set; }
        public DateTime? FechaEdicion { get; set; }

        public virtual ProyectoAdministrativo ProyectoAdminstrativo { get; set; }
        public virtual ICollection<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
    }
}
