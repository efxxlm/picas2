﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionMonitoreo
    {
        public int ProcesoSeleccionMonitoreoId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string NumeroProceso { get; set; }
        public string EstadoActividadCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? EnviadoComiteTecnico { get; set; }
    }
}
