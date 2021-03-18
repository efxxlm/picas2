﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VUsuarioPerfil
    {
        public int UsuarioPerfilId { get; set; }
        public int PerfilId { get; set; }
        public int UsuarioId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
