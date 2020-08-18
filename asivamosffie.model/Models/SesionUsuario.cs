using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionUsuario
    {
        public int SesionUsuarioId { get; set; }
        public int UsuarioId { get; set; }
        public int SesionId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual Sesion Sesion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
