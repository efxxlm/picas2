using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Usuario
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public bool? Activo { get; set; }
        public bool? Bloqueado { get; set; }
        public int? IntentosFallidos { get; set; }
        public bool? Eliminado { get; set; }
        public string NombreMaquina { get; set; }
        public string Ip { get; set; }
        public string IpProxy { get; set; }
        public DateTime? FechaUltimoIngreso { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
