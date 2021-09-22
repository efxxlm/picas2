using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDrpXcontratoXaportante
    {
        public int ContratoId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public decimal? ValorUso { get; set; }
        public string NumeroDrp { get; set; }
    }
}
