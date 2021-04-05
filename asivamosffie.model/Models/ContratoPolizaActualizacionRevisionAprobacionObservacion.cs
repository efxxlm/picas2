using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPolizaActualizacionRevisionAprobacionObservacion
    {
        public int ContratoPolizaActualizacionRevisionAprobacionObservacionId { get; set; }
        public int? ContratoPolizaActualizacionId { get; set; }
        public DateTime? SegundaFechaRevision { get; set; }
        public string EstadoSegundaRevision { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int? ResponsableAprobacionId { get; set; }
        public string ObservacionGeneral { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Archivada { get; set; }

        public virtual ContratoPolizaActualizacion ContratoPolizaActualizacion { get; set; }
        public virtual Usuario ResponsableAprobacion { get; set; }
    }
}
