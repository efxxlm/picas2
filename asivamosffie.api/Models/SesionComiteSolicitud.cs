using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
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
        public bool? RequiereVotacion { get; set; }
        public int? ComiteTecnicoFiduciarioId { get; set; }
        public DateTime? FechaComiteFiduciario { get; set; }
        public string UsuarioComiteFiduciario { get; set; }
        public string EstadoActaCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string DesarrolloSolicitud { get; set; }
        public string DesarrolloSolicitudFiduciario { get; set; }
        public string EstadoActaCodigoFiduciario { get; set; }
        public string ObservacionesFiduciario { get; set; }
        public string RutaSoporteVotacionFiduciario { get; set; }
        public bool? GeneraCompromisoFiduciario { get; set; }
        public int? CantCompromisosFiduciario { get; set; }
        public bool? RequiereVotacionFiduciario { get; set; }
        public bool? RegistroCompletoFiduciaria { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual ComiteTecnico ComiteTecnicoFiduciario { get; set; }
        public virtual ICollection<SesionSolicitudCompromiso> SesionSolicitudCompromiso { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual ICollection<SesionSolicitudVoto> SesionSolicitudVoto { get; set; }
    }
}
