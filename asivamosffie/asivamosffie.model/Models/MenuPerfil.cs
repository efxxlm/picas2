using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class MenuPerfil
    {
        public int MenuPerfilId { get; set; }
        public int MenuId { get; set; }
        public int PerfilId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Activo { get; set; }
        public bool Crud { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
