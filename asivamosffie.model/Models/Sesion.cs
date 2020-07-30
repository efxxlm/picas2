using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Sesion
    {
        public Sesion()
        {
            SesionComentario = new HashSet<SesionComentario>();
            SesionComiteTecnico = new HashSet<SesionComiteTecnico>();
            SesionComiteTema = new HashSet<SesionComiteTema>();
            SesionInvitado = new HashSet<SesionInvitado>();
        }

        public int SesionId { get; set; }
        public DateTime FechaOrdenDia { get; set; }
        public string NumeroComite { get; set; }
        public string EstadoComiteCodigo { get; set; }
        public bool? EsCompleto { get; set; }
        public string RutaActaSesion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<SesionComentario> SesionComentario { get; set; }
        public virtual ICollection<SesionComiteTecnico> SesionComiteTecnico { get; set; }
        public virtual ICollection<SesionComiteTema> SesionComiteTema { get; set; }
        public virtual ICollection<SesionInvitado> SesionInvitado { get; set; }
    }
}
