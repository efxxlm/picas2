using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoObservacion
    {
        public int SolicitudPagoObservacionId { get; set; }
        public int SolicitudPagoId { get; set; }
        public string Observacion { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public string PerfilObservacionCodigo { get; set; }
        public bool? Archivada { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? IdPadre { get; set; }
        public bool TieneObservacion { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
