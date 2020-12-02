using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class MensajesValidaciones
    {
        public MensajesValidaciones()
        {
            Auditoria = new HashSet<Auditoria>();
        }

        public int MensajesValidacionesId { get; set; }
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public int? MenuId { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual ICollection<Auditoria> Auditoria { get; set; }
    }
}
