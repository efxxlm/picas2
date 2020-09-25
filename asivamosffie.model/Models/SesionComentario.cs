using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
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
        public string EstadoActaVoto { get; set; }
        public bool ValidacionVoto { get; set; }

    public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Usuario MiembroSesionParticipante { get; set; }
    }
}
