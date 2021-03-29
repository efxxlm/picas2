using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseFacturaDescuento
    {
        public SolicitudPagoFaseFacturaDescuento()
        {
            OrdenGiroDetalleDescuentoTecnicaAportante = new HashSet<OrdenGiroDetalleDescuentoTecnicaAportante>();
        }

        public int SolicitudPagoFaseFacturaDescuentoId { get; set; }
        public int SolicitudPagoFaseFacturaId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public decimal? ValorDescuento { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? RegistroCompletoSupervisor { get; set; }
        public bool? RegistroCompletoCoordinador { get; set; }

        public virtual SolicitudPagoFaseFactura SolicitudPagoFaseFactura { get; set; }
        public virtual ICollection<OrdenGiroDetalleDescuentoTecnicaAportante> OrdenGiroDetalleDescuentoTecnicaAportante { get; set; }
    }
}
