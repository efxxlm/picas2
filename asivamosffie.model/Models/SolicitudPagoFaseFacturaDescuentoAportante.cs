using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseFacturaDescuentoAportante
    {
        public int SolicitudPagoFaseFacturaDescuentoAportanteId { get; set; }
        public int SolicitudPagoFaseFacturaDescuentoId { get; set; }
        public decimal? ValorDescuento { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPagoFaseFacturaDescuento SolicitudPagoFaseFacturaDescuento { get; set; }
    }
}
