﻿using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaSesionComiteTecnicoCompromiso
    {
        public int ComiteTecnicoId { get; set; }
        public DateTime? FechaOrdenDia { get; set; }
        public string  NumeroComite { get; set; }
        public string Tarea { get; set; }
        public int SesionComiteTecnicoCompromisoId { get; set; }
        public DateTime? FechaCumplimiento { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoCompromisoText { get; set; }
        public string EstadoComiteText { get; set; }
        public string EstadoComiteCodigo { get; set; }

    }
}
