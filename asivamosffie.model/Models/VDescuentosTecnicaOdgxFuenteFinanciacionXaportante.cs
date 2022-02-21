﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDescuentosTecnicaOdgxFuenteFinanciacionXaportante
    {
        public int? OrdenGiroId { get; set; }
        public int? AportanteId { get; set; }
        public string Nombre { get; set; }
        public string TipoRecursosCodigo { get; set; }
        public int? ContratacionId { get; set; }
        public int? ContratoId { get; set; }
        public int? ValorDescuento { get; set; }
        public int SolicitudPagoId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
    }
}
