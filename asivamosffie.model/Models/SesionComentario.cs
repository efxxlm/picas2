using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComentario
    {
        public int SesionComentarioId { get; set; }
        public int SesionId { get; set; }
        public DateTime Fecha { get; set; }
        public string Miembro { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Sesion Sesion { get; set; }
    }
}
