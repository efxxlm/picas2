﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VTablaOdgOtroDescuento
    {
        public int OrdenGiroId { get; set; }
        public string ConceptoPago { get; set; }
        public string Descuento { get; set; }
        public string DescuentoCodigo { get; set; }
        public int? AportanteId { get; set; }
        public int ValorDescuento { get; set; }
    }
}
