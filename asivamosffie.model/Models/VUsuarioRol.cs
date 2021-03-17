using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VUsuarioRol
    {
        public DateTime? FechaCreacion { get; set; }
        public string ProcedenciaCodigo { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Email { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Rol { get; set; }
        public bool? Eliminado { get; set; }
        public int UsuarioId { get; set; }
    }
}
