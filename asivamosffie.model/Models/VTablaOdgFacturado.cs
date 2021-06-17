﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VTablaOdgFacturado
    {
        public int OrdenGiroId { get; set; }
        public int? AportanteId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public string ConceptoPago { get; set; }
        public string Uso { get; set; }
        public string TipoPago { get; set; }
    }
}
