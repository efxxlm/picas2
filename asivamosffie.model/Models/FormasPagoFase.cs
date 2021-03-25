using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FormasPagoFase
    {
        public int FormasPagoFaseId { get; set; }
        public string FormaPagoCodigo { get; set; }
        public bool EsPreconstruccion { get; set; }
    }
}
