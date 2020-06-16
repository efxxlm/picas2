using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Template
    {
        public int TemplateId { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
