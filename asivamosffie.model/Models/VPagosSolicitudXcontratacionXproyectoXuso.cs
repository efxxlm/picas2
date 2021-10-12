﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VPagosSolicitudXcontratacionXproyectoXuso
    {
        public bool EstaAprobadaOdg { get; set; }
        public bool? Pagado { get; set; }
        public int ContratacionId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public string NumeroDrp { get; set; }
        public string TipoUsoCodigo { get; set; }
        public string Uso { get; set; }
        public bool EsPreconstruccion { get; set; }
        public decimal? SaldoUso { get; set; }
        public int ConceptoPagoCriterio { get; set; }
        public int SolicitudPagoFaseCriterioConceptoPagoId { get; set; }
    }
}
