using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SolicitudPagoOtrosCostosServicios
    {
        public int SolicitudPagoOtrosCostosServiciosId { get; set; }
        public int SolicitudPagoId { get; set; }
        public string NumeroRadicadoSac { get; set; }
        public string NumeroFactura { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string TipoPagoCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
