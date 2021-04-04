using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratacionProyectoSolicitudLiquidacion
    {
        public DateTime? FechaPoliza { get; set; }
        public string NumeroContrato { get; set; }
        public decimal ValorSolicitud { get; set; }
        public int? ProyectosAsociados { get; set; }
        public string EstadoVerificacionLiquidacionCodigo { get; set; }
        public string EstadoVerificacionLiquidacionString { get; set; }
        public string NumeroSolicitudLiquidacion { get; set; }
    }
}
