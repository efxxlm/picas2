using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteInvitadoVoto
    {
        public int SesionComiteInvitadoVotoId { get; set; }
        public int SesionComiteTecnicoId { get; set; }
        public int SesionInvitadoId { get; set; }
        public bool EsAprobado { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesDevolucion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SesionComiteTecnico SesionComiteTecnico { get; set; }
        public virtual SesionInvitado SesionInvitado { get; set; }
    }
}
