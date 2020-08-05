using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestalProyecto
    {
        public int DisponibilidadPresupuestalProyectoId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        
        
        

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
