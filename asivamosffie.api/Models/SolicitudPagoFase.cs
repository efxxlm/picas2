using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SolicitudPagoFase
    {
        public SolicitudPagoFase()
        {
            SolicitudPagoFaseAmortizacion = new HashSet<SolicitudPagoFaseAmortizacion>();
            SolicitudPagoFaseCriterio = new HashSet<SolicitudPagoFaseCriterio>();
            SolicitudPagoFaseFactura = new HashSet<SolicitudPagoFaseFactura>();
        }

        public int SolicitudPagoFaseId { get; set; }
        public bool EsPreconstruccion { get; set; }
        public int SolicitudPagoRegistrarSolicitudPagoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPagoRegistrarSolicitudPago SolicitudPagoRegistrarSolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoFaseAmortizacion> SolicitudPagoFaseAmortizacion { get; set; }
        public virtual ICollection<SolicitudPagoFaseCriterio> SolicitudPagoFaseCriterio { get; set; }
        public virtual ICollection<SolicitudPagoFaseFactura> SolicitudPagoFaseFactura { get; set; }
    }
}
