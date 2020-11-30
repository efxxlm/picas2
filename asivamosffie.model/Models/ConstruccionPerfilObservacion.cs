using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionPerfilObservacion
    {
        public int ConstruccionPerfilObservacionId { get; set; }
        public int ConstruccionPerfilId { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? EsSupervision { get; set; }
        public bool? Archivada { get; set; }

        public virtual ConstruccionPerfil ConstruccionPerfil { get; set; }
    }
}
