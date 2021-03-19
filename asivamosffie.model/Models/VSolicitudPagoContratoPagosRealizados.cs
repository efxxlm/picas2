﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSolicitudPagoContratoPagosRealizados
    {
        public int SolicitudPagoId { get; set; }
        public int ContratoId { get; set; }
        public bool EsPreconstruccion { get; set; }
        public string FaseContrato { get; set; }
        public int? PagosRealizados { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal? PorcentajeFacturado { get; set; }
        public decimal? SaldoPorPagar { get; set; }
        public decimal? PorcentajePorPagar { get; set; }
    }
}
