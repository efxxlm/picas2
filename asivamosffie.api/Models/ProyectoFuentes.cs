using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ProyectoFuentes
    {
        public int ProyectoFuenteId { get; set; }
        public int ProyectoId { get; set; }
        public int FuenteId { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual FuenteFinanciacion Fuente { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
