using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ContratoPerfilObservacion
    {
        public int ContratoPerfilObservacionId { get; set; }
        public int ContratoPerfilId { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ContratoPerfil ContratoPerfil { get; set; }
    }
}
