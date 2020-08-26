using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteSolicitud
    {
        public SesionComiteSolicitud()
        {
            SesionSolicitudCompromiso = new HashSet<SesionSolicitudCompromiso>();
            SesionSolicitudObservacionProyecto = new HashSet<SesionSolicitudObservacionProyecto>();
            SesionSolicitudVoto = new HashSet<SesionSolicitudVoto>();
        }

        public int SesionComiteSolicitudId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int SolicitudId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int ComiteTecnicoId { get; set; }
        public string EstadoCodigo { get; set; }
        public string Observaciones { get; set; }
        public string RutaSoporteVotacion { get; set; }
        public bool? GeneraCompromiso { get; set; }
        public int? CantCompromisos { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual ICollection<SesionSolicitudCompromiso> SesionSolicitudCompromiso { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual ICollection<SesionSolicitudVoto> SesionSolicitudVoto { get; set; }
    }
}
