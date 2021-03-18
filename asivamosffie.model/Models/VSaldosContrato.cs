using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldosContrato
    {
        public int? ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public decimal? SaldoPresupuestal { get; set; }
        public decimal? SaldoTesoral { get; set; }
    }
}
