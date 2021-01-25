using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SolicitudPagoFaseAmortizacion
    {
        public int SolicitudPagoFaseAmortizacionId { get; set; }
        public int? SolicitudPagoFaseId { get; set; }
        public int? PorcentajeAmortizacion { get; set; }
        public decimal? ValorAmortizacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPagoFase SolicitudPagoFase { get; set; }
    }
}
