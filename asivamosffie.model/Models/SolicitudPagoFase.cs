using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFase
    {
        public SolicitudPagoFase()
        {
            SolicitudAmortizacion = new HashSet<SolicitudAmortizacion>();
            SolicitudPagoFaseCriterio = new HashSet<SolicitudPagoFaseCriterio>();
            SolicitudPagoFaseFactura = new HashSet<SolicitudPagoFaseFactura>();
        }

        public int SolicitudPagoFaseId { get; set; }
        public bool EsPreconstruccion { get; set; }

        public virtual ICollection<SolicitudAmortizacion> SolicitudAmortizacion { get; set; }
        public virtual ICollection<SolicitudPagoFaseCriterio> SolicitudPagoFaseCriterio { get; set; }
        public virtual ICollection<SolicitudPagoFaseFactura> SolicitudPagoFaseFactura { get; set; }
    }
}
