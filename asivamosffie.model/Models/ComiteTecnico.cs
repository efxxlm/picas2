using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComiteTecnico
    {
        public ComiteTecnico()
        {
            ContratacionObservacion = new HashSet<ContratacionObservacion>();
            SesionComentario = new HashSet<SesionComentario>();
            SesionComiteSolicitudComiteTecnico = new HashSet<SesionComiteSolicitud>();
            SesionComiteSolicitudComiteTecnicoFiduciario = new HashSet<SesionComiteSolicitud>();
            SesionComiteTecnicoCompromiso = new HashSet<SesionComiteTecnicoCompromiso>();
            SesionComiteTema = new HashSet<SesionComiteTema>();
            SesionInvitado = new HashSet<SesionInvitado>();
            SesionParticipante = new HashSet<SesionParticipante>();
            SesionParticipanteVoto = new HashSet<SesionParticipanteVoto>();
            SesionResponsable = new HashSet<SesionResponsable>();
        }

        public int ComiteTecnicoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? EsCompleto { get; set; }
        public bool? RequiereVotacion { get; set; }
        public string Justificacion { get; set; }
        public bool? EsAprobado { get; set; }
        public DateTime? FechaAplazamiento { get; set; }
        public string Observaciones { get; set; }
        public string RutaSoporteVotacion { get; set; }
        public bool? TieneCompromisos { get; set; }
        public int? CantCompromisos { get; set; }
        public string RutaActaSesion { get; set; }
        public DateTime? FechaOrdenDia { get; set; }
        public string NumeroComite { get; set; }
        public string EstadoComiteCodigo { get; set; }
        public string EstadoActaCodigo { get; set; }
        public bool? EsComiteFiduciario { get; set; }
        public string TipoTemaFiduciarioCodigo { get; set; }

        public virtual ICollection<ContratacionObservacion> ContratacionObservacion { get; set; }
        public virtual ICollection<SesionComentario> SesionComentario { get; set; }
        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitudComiteTecnico { get; set; }
        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitudComiteTecnicoFiduciario { get; set; }
        public virtual ICollection<SesionComiteTecnicoCompromiso> SesionComiteTecnicoCompromiso { get; set; }
        public virtual ICollection<SesionComiteTema> SesionComiteTema { get; set; }
        public virtual ICollection<SesionInvitado> SesionInvitado { get; set; }
        public virtual ICollection<SesionParticipante> SesionParticipante { get; set; }
        public virtual ICollection<SesionParticipanteVoto> SesionParticipanteVoto { get; set; }
        public virtual ICollection<SesionResponsable> SesionResponsable { get; set; }
    }
}
