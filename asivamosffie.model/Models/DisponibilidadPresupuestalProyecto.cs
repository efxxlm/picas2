using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestalProyecto
    {
        public int DisponibilidadPresupuestalProyectoId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int ProyectoId { get; set; }

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
