using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoMonitoreoWeb
    {
        public int ProyectoMonitoreoWebId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Proyecto Proyecto { get; set; }
    }
}
