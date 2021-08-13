﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProcesoSeleccionReporteHist
    {
        public int ProcesoSeleccionId { get; set; }
        public string NumeroProceso { get; set; }
        public string Objeto { get; set; }
        public string AlcanceParticular { get; set; }
        public string Justificacion { get; set; }
        public string CriteriosSeleccion { get; set; }
        public string TipoIntervencionCodigo { get; set; }
        public string TipoAlcanceCodigo { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public bool? EsDistribucionGrupos { get; set; }
        public int? CantGrupos { get; set; }
        public int? ResponsableTecnicoUsuarioId { get; set; }
        public int? ResponsableEstructuradorUsuarioid { get; set; }
        public string CondicionesJuridicasHabilitantes { get; set; }
        public string CondicionesFinancierasHabilitantes { get; set; }
        public string CondicionesTecnicasHabilitantes { get; set; }
        public string CondicionesAsignacionPuntaje { get; set; }
        public int? CantidadCotizaciones { get; set; }
        public int? CantidadProponentes { get; set; }
        public bool? EsCompleto { get; set; }
        public string EstadoProcesoSeleccionCodigo { get; set; }
        public string EtapaProcesoSeleccionCodigo { get; set; }
        public string EvaluacionDescripcion { get; set; }
        public string UrlSoporteEvaluacion { get; set; }
        public string TipoOrdenEligibilidadCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? CantidadProponentesInvitados { get; set; }
        public string UrlSoporteProponentesSeleccionados { get; set; }
        public int? SolicitudId { get; set; }
    }
}
