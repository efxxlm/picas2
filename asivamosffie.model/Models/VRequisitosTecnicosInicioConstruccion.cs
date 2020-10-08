﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRequisitosTecnicosInicioConstruccion
    {
        public int ContratoId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroContrato { get; set; }
        public int? CantidadProyectosAsociados { get; set; }
        public int? CantidadProyectosRequisitosAprobados { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public bool? ExisteRegistro { get; set; }
    }
}
