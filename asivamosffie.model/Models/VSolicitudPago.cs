﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSolicitudPago
    {
        public string TipoSolicitudCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NumeroSolicitud { get; set; }
        public int? ContratoId { get; set; }
        public int SolicitudPagoId { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public string NumeroContrato { get; set; }
        public string ModalidadNombre { get; set; }
        public bool? RegistroCompletoVerificar { get; set; }
        public bool? RegistroCompletoAutorizar { get; set; }
    }
}
