using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteTecnico
    {
        public SesionComiteTecnico()
        {
            SesionComiteInvitadoVoto = new HashSet<SesionComiteInvitadoVoto>();
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
            SesionComiteTecnicoCompromiso = new HashSet<SesionComiteTecnicoCompromiso>();
        }

        public int SesionComiteTecnicoId { get; set; }
        public int SesionId { get; set; }
        public int? ComiteTecnicoId { get; set; }
        public bool? RequiereVotacion { get; set; }
        public string Justificacion { get; set; }
        public bool? EsAprobado { get; set; }
        public string Observaciones { get; set; }
        public string RutaSoporteVotacion { get; set; }
        public bool? TieneCompromisos { get; set; }
        public int? CantCompromisos { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Sesion Sesion { get; set; }
        public virtual ICollection<SesionComiteInvitadoVoto> SesionComiteInvitadoVoto { get; set; }
        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
        public virtual ICollection<SesionComiteTecnicoCompromiso> SesionComiteTecnicoCompromiso { get; set; }
    }
}
