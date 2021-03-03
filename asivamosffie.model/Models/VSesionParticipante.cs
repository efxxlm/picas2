using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSesionParticipante
    {
        public int SesionParticipanteId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroIdentificacion { get; set; }
    }
}
