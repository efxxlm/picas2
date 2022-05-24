﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VGestionarGarantiasPolizas
    {
        public int? SupervisorId { get; set; }
        public int? ApoyoId { get; set; }
        public int? InterventorId { get; set; }
        public int ContratoId { get; set; }
        public int? ContratoPolizaId { get; set; }
        public DateTime? FechaFirma { get; set; }
        public DateTime? FechaCreacionContrato { get; set; }
        public string NumeroContrato { get; set; }
        public string NumeroSolicitudContratacion { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoSolicitudCodigoContratacion { get; set; }
        public string TipoSolicitudContratacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? RegistroCompletoPoliza { get; set; }
        public string EstadoPoliza { get; set; }
        public string EstadoPolizaCodigo { get; set; }
        public string RegistroCompletoNombre { get; set; }
        public string RegistroCompletoPolizaNombre { get; set; }
    }
}
