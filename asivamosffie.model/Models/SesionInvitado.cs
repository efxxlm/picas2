using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionInvitado
    {
        public int SesionInvitadoId { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Entidad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? ComiteTecnicoId { get; set; }
<<<<<<< HEAD

        public virtual ComiteTecnico ComiteTecnico { get; set; }
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
    }
}
