using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteTecnico
    {
        public SesionComiteTecnico()
        {
            SesionComiteInvitadoVoto = new HashSet<SesionComiteInvitadoVoto>();
            SesionComiteTecnicoCompromiso = new HashSet<SesionComiteTecnicoCompromiso>();
        }

        public int SesionComiteTecnicoId { get; set; }
        public int SesionId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public bool? RequiereVotacion { get; set; }
        public string Justificacion { get; set; }
        public bool? EsAprobado { get; set; }
        public string Observaciones { get; set; }
        public string RutaSoporteVotacion { get; set; }
        public bool? TieneCompromisos { get; set; }
        public int? CantCompromisos { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Sesion Sesion { get; set; }
        public virtual ICollection<SesionComiteInvitadoVoto> SesionComiteInvitadoVoto { get; set; }
        public virtual ICollection<SesionComiteTecnicoCompromiso> SesionComiteTecnicoCompromiso { get; set; }
    }
}
