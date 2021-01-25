using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            MenuPerfil = new HashSet<MenuPerfil>();
        }

        public int PerfilId { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<MenuPerfil> MenuPerfil { get; set; }
    }
}
