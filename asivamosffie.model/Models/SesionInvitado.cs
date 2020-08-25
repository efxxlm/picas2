using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionInvitado
    {
        public SesionInvitado()
        {
            SesionComiteInvitadoVoto = new HashSet<SesionComiteInvitadoVoto>();
        }

        public int SesionInvitadoId { get; set; }
        public int SesionId { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Entidad { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Sesion Sesion { get; set; }
        public virtual ICollection<SesionComiteInvitadoVoto> SesionComiteInvitadoVoto { get; set; }
    }
}
