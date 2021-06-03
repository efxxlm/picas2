﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOrdenGiro
    {
        public DateTime? FechaPagoFiduciaria { get; set; }
        public DateTime? FechaRegistroCompletoAprobar { get; set; }
        public DateTime? FechaAprobacionFinanciera { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string NumeroSolicitudPago { get; set; }
        public string NumeroSolicitudOrdenGiro { get; set; }
        public int? OrdenGiroId { get; set; }
        public bool? TieneObservacion { get; set; }
        public string Modalidad { get; set; }
        public string NumeroContrato { get; set; }
        public int SolicitudPagoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitud { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoNombre2 { get; set; }
        public string EstadoCodigo { get; set; }
        public int IntEstadoCodigo { get; set; }
        public bool RegistroCompleto { get; set; }
        public bool RegistroCompletoVerificar { get; set; }
        public bool RegistroCompletoAprobar { get; set; }
        public bool RegistroCompletoTramitar { get; set; }
        public DateTime? FechaRegistroCompletoTramitar { get; set; }
    }
}
