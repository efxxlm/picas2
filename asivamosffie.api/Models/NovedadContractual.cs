﻿using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class NovedadContractual
    {
        public int NovedadContractualId { get; set; }
        public DateTime FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string InstanciaCodigo { get; set; }
        public DateTime FechaSesionInstancia { get; set; }
        public string TipoNovedadCodigo { get; set; }
        public string MotivoNovedadCodigo { get; set; }
        public string ResumenJustificacion { get; set; }
        public bool EsDocumentacionSoporte { get; set; }
        public string ConceptoTecnico { get; set; }
        public DateTime? FechaConcepto { get; set; }
        public DateTime? FechaInicioSuspension { get; set; }
        public DateTime? FechaFinSuspension { get; set; }
        public decimal? PresupuestoAdicionalSolicitado { get; set; }
        public int? PlazoAdicionalDias { get; set; }
        public int? PlazoAdicionalMeses { get; set; }
        public string ClausulaModificar { get; set; }
        public string AjusteClausula { get; set; }
        public int SolicitudId { get; set; }
    }
}