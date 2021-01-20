using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoRegistrarSolicitudPago
    {
        public SolicitudPagoRegistrarSolicitudPago()
        {
            SolicitudPagoFase = new HashSet<SolicitudPagoFase>();
            SolicitudPagoFaseDescuento = new HashSet<SolicitudPagoFaseDescuento>();
        }

        public int SolicitudPagoRegistrarSolicitudPagoId { get; set; }
        public int SolicitudPagoId { get; set; }
        public bool? TieneFasePreconstruccion { get; set; }
        public bool? TieneFaseConstruccion { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroRadicadoSac { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoFase> SolicitudPagoFase { get; set; }
        public virtual ICollection<SolicitudPagoFaseDescuento> SolicitudPagoFaseDescuento { get; set; }
    }
}
