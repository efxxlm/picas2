using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoAdministrativo
    {
        public ProyectoAdministrativo()
        {
            DisponibilidadPresupuestalProyecto = new HashSet<DisponibilidadPresupuestalProyecto>();
            ProyectoAdministrativoAportante = new HashSet<ProyectoAdministrativoAportante>();
        }

        public int ProyectoAdministrativoId { get; set; }
        public bool? Enviado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual ICollection<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
        public virtual ICollection<ProyectoAdministrativoAportante> ProyectoAdministrativoAportante { get; set; }
    }
}
