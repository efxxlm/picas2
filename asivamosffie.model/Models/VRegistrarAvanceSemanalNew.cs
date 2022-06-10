using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarAvanceSemanalNew
    {
        public int ContratacionProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public int? SeguimientoSemanalId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int ContratoId { get; set; }
        public string FechaUltimoReporte { get; set; }
        public string EstadoObra { get; set; }
        public string EstadoObraCodigo { get; set; }
        public bool? ActaCargada { get; set; }
        public bool? VerEditarBitacora { get; set; }
        public int? CantidadSemanas { get; set; }
        public int? NumeroSemana { get; set; }
        public bool? CumpleCondicionesTai { get; set; }
        public bool? VerReportarSeguimientoSemanal { get; set; }
        public bool? VerCargarActa { get; set; }
        public bool? EnviarVerificacion { get; set; }
        public DateTime? FechaModificacionAvalar { get; set; }
        public DateTime? FechaModificacionVerificar { get; set; }
        public bool? ExisteReprogramacionActiva { get; set; }
    }
}
