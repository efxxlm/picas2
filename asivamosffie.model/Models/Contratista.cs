using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contratista
    {
        public Contratista()
        {
            Contratacion = new HashSet<Contratacion>();
        }

        public int ContratistaId { get; set; }
        public string TipoIdentificacionCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string RepresentanteLegal { get; set; }
        public string NumeroInvitacion { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string RepresentanteLegalNumeroIdentificacion { get; set; }

        public virtual ICollection<Contratacion> Contratacion { get; set; }
    }
}
