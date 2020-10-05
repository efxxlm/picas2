using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class PolizaGarantia
    {
        public int PolizaGarantiaId { get; set; }
        public int ContratoPolizaId { get; set; }
        public string TipoGarantiaCodigo { get; set; }
        public bool EsIncluidaPoliza { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
    }
}
