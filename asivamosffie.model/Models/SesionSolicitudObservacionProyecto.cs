using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionSolicitudObservacionProyecto
    {
        public int SesionSolicitudObservacionProyectoId { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int? SesionParticipanteId { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
<<<<<<< HEAD
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual SesionComiteSolicitud SesionComiteSolicitud { get; set; }
        public virtual SesionParticipante SesionParticipante { get; set; }
    }
}
