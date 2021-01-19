using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPago
    {
        public SolicitudPago()
        {
            SolicitudPagoCargarFormaPago = new HashSet<SolicitudPagoCargarFormaPago>();
            SolicitudPagoExpensas = new HashSet<SolicitudPagoExpensas>();
            SolicitudPagoOtrosCostosServicios = new HashSet<SolicitudPagoOtrosCostosServicios>();
            SolicitudPagoRegistrarSolicitudPago = new HashSet<SolicitudPagoRegistrarSolicitudPago>();
            SolicitudPagoSoporteSolicitud = new HashSet<SolicitudPagoSoporteSolicitud>();
        }

        public int SolicitudPagoId { get; set; }
        public string TipoSolicitud { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? ListaChequeoId { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<SolicitudPagoCargarFormaPago> SolicitudPagoCargarFormaPago { get; set; }
        public virtual ICollection<SolicitudPagoExpensas> SolicitudPagoExpensas { get; set; }
        public virtual ICollection<SolicitudPagoOtrosCostosServicios> SolicitudPagoOtrosCostosServicios { get; set; }
        public virtual ICollection<SolicitudPagoRegistrarSolicitudPago> SolicitudPagoRegistrarSolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoSoporteSolicitud> SolicitudPagoSoporteSolicitud { get; set; }
    }
}
