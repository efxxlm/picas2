using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRendimientodXcuentaBancaria
    {
        public string CuentaBancaria { get; set; }
        public decimal? TotalRendimientos { get; set; }
        public decimal? RendimientoIncorporar { get; set; }
    }
}
