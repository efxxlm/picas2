using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SesionComentario
    {
        public int SesionComentarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int MiembroSesionParticipanteId { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual SesionParticipante MiembroSesionParticipante { get; set; }
    }
}
