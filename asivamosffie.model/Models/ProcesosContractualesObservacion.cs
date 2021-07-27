using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesosContractualesObservacion
    {
        public int ProcesosContractualesObservacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int? SolicitudId { get; set; }
        public bool? TieneObservacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string Observacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string TipoObservacionCodigo { get; set; }
    }
}
