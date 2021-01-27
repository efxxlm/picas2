using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiro
    {
        public int OrdenGiroId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public int? OrdenGiroTerceroId { get; set; }
        public int? OrdenGiroDetalleId { get; set; }
        public int? OrdenGiroObservacionId { get; set; }
        public int? OrdenGiroSoporteId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
        public virtual OrdenGiroTercero OrdenGiroTercero { get; set; }
        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
