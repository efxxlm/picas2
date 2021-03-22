using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProyectoReporteHist
    {
        public int ProyectoId { get; set; }
        public DateTime? FechaSesionJunta { get; set; }
        public int? NumeroActaJunta { get; set; }
        public string TipoIntervencionCodigo { get; set; }
        public string LlaveMen { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public int? InstitucionEducativaId { get; set; }
        public int? SedeId { get; set; }
        public bool? EnConvocatoria { get; set; }
        public int? ConvocatoriaId { get; set; }
        public int? CantPrediosPostulados { get; set; }
        public string TipoPredioCodigo { get; set; }
        public int? PredioPrincipalId { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotal { get; set; }
        public string EstadoProyectoCodigoOld { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string EstadoJuridicoCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? TieneEstadoFase1EyD { get; set; }
        public bool? TieneEstadoFase1Diagnostico { get; set; }
        public string UrlMonitoreo { get; set; }
        public string EstadoProgramacionCodigo { get; set; }
        public int? PlazoMesesObra { get; set; }
        public int? PlazoDiasObra { get; set; }
        public int? PlazoMesesInterventoria { get; set; }
        public int? PlazoDiasInterventoria { get; set; }
        public string CoordinacionResponsableCodigo { get; set; }
        public string EstadoProyectoObraCodigo { get; set; }
        public string EstadoProyectoInterventoriaCodigo { get; set; }
    }
}
