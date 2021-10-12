﻿using System;
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
        public string EstadoAprobacionLiquidacionCodigo { get; set; }
        public string EstadoAprobacionLiquidacionString { get; set; }
        public string EstadoTramiteLiquidacion { get; set; }
        public string EstadoTramiteLiquidacionString { get; set; }
        public string NumeroSolicitudLiquidacion { get; set; }
        public DateTime? FechaValidacionLiquidacion { get; set; }
        public DateTime? FechaAprobacionLiquidacion { get; set; }
        public DateTime? FechaTramiteLiquidacionControl { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ContratacionId { get; set; }
        public int? ContratoId { get; set; }
        public int? ContratoPolizaId { get; set; }
        public int? ContratoPolizaActualizacionId { get; set; }
        public bool? CumpleCondicionesTai { get; set; }
    }
}
