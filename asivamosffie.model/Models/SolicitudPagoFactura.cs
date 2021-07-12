using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFactura
    {
        public int SolicitudPagoFacturaId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string Numero { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? TieneDescuento { get; set; }
        public decimal? ValorFacturadoConDescuento { get; set; }
        public bool? RegistroCompletoSupervisor { get; set; }
        public bool? RegistroCompletoCoordinador { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
