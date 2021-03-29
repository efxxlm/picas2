using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOrdenGiro
    {
        public DateTime? FechaAprobacionFinanciera { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string NumeroSolicitudPago { get; set; }
        public string NumeroSolicitudOrdenGiro { get; set; }
        public int? OrdenGiroId { get; set; }
        public string Modalidad { get; set; }
        public string NumeroContrato { get; set; }
        public int SolicitudPagoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoNombre2 { get; set; }
        public int? IntEstadoCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
    }
}
