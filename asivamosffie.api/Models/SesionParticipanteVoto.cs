using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SesionParticipanteVoto
    {
        public int SesionParticipanteVotoId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int SesionParticipanteId { get; set; }
        public bool EsAprobado { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesDevolucion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual SesionParticipante SesionParticipante { get; set; }
    }
}
