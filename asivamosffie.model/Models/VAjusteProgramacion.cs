using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAjusteProgramacion
    {
        public DateTime? FechaAprobacionPoliza { get; set; }
        public string NumeroContrato { get; set; }
        public string LlaveMen { get; set; }
        public string NovedadesSeleccionadas { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoCodigo { get; set; }
        public int? AjusteProgramacionId { get; set; }
        public bool? TieneObservacionesFlujoInversion { get; set; }
        public bool? TieneObservacionesProgramacionObra { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? InterventorId { get; set; }
        public int? ApoyoId { get; set; }
        public int? SupervisorId { get; set; }
        public string NombreContratista { get; set; }
    }
}
