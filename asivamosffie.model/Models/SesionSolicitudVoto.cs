using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionSolicitudVoto
    {
        public int SesionSolicitudVotoId { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public int SesionParticipanteId { get; set; }
        public bool? EsAprobado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual SesionComiteSolicitud SesionComiteSolicitud { get; set; }
        public virtual SesionParticipante SesionParticipante { get; set; }
    }
}
