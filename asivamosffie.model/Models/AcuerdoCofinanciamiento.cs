using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AcuerdoCofinanciamiento
    {
        public int AcuerdoCofinanciacionId { get; set; }
        public string VigenciaCodigo { get; set; }
        public int CantidadAportantes { get; set; }
        public decimal ValorTotal { get; set; }
        public string EstadoCodigo { get; set; }
    }
}
