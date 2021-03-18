﻿using System;
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
        public bool? Activo { get; set; }
        public bool? TienePermisoCrear { get; set; }
        public bool? TienePermisoLeer { get; set; }
        public bool? TienePermisoEditar { get; set; }
        public bool? TienePermisoEliminar { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
