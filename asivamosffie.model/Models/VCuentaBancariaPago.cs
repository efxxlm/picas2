using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.Models
{
    public partial class VCuentaBancariaPago
    {
        public string TipoCriterioCodigo { get; set; }
        public int SolicitudPagoId { get; set; }
        public int OrdenGiroId { get; set; }
        public int OrdenGiroDetalleTerceroCausacionId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string NumeroCuentaBanco { get; set; }
        public decimal? ValorNetoGiro { get; set; }
    }
}
