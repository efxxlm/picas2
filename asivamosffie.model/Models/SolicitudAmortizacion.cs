using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudAmortizacion
    {
        public int SolicitudPagoFaseAmortizacionAnticipoId { get; set; }
        public int? SolicitudPagoFaseId { get; set; }
        public int? PorcentajeAmortizacion { get; set; }
        public decimal? ValorAmortizacion { get; set; }

        public virtual SolicitudPagoFase SolicitudPagoFase { get; set; }
    }
}
