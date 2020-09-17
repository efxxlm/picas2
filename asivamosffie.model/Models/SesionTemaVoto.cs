using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionTemaVoto
    {
        public int SesionTemaVotoId { get; set; }
        public int SesionTemaId { get; set; }
        public int SesionParticipanteId { get; set; }
<<<<<<< HEAD
        public bool? EsAprobado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
=======
        public bool EsAprobado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a

        public virtual SesionParticipante SesionParticipante { get; set; }
        public virtual SesionComiteTema SesionTema { get; set; }
    }
}
