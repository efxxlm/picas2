using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VNombreCuentaXodgXaportanteXconcepto
    {
        public int OrdenGiroId { get; set; }
        public int? AportanteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public string CodigoSifi { get; set; }
        public string TipoCuenta { get; set; }
        public string NombreBanco { get; set; }
        public int OrdenGiroDetalleTerceroCausacionAportanteId { get; set; }
        public string ConceptoCodigo { get; set; }
        public int? FuenteFinanciacionId { get; set; }
    }
}
