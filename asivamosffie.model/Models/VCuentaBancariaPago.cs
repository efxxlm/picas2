using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VCuentaBancariaPago
    {
        public string TipoPagoCodigo { get; set; }
        public int SolicitudPagoId { get; set; }
        public int? OrdenGiroId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string NumeroCuentaBanco { get; set; }
        public decimal? ValorNetoGiro { get; set; }
    }
}
