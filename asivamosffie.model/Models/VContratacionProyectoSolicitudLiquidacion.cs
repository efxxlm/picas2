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
        public string EstadoValidacionLiquidacionCodigo { get; set; }
        public string EstadoValidacionLiquidacionString { get; set; }
        public string NumeroSolicitudLiquidacion { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public int InformeFinalId { get; set; }
    }
}
