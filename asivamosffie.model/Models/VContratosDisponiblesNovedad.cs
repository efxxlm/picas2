using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratosDisponiblesNovedad
    {
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NombreContratista { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int PlazoMeses { get; set; }
        public int PlazoDias { get; set; }
        public DateTime? FechaActaInicioFase1 { get; set; }
        public DateTime? FechaActaInicioFase2 { get; set; }
        public int ContratacionId { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        public DateTime? FechaTerminacionFase2 { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public bool? EsMultiProyecto { get; set; }
        public bool? TieneActa { get; set; }
        public bool? CumpleCondicionesTai { get; set; }
        public bool? TieneSuspensionAprobada { get; set; }
    }
}
