﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VNovedadContractual
    {
        public int NovedadContractualId { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoCodigoNombre { get; set; }
        public bool? TieneObservacionesApoyo { get; set; }
        public bool? TieneObservacionesSupervisor { get; set; }
        public string NovedadesSeleccionadas { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroContrato { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }
    }
}
