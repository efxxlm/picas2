using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VActualizacionPolizaYgarantias
    {
        public DateTime? FechaExpedicionActualizacionPoliza { get; set; }
        public string NumeroContrato { get; set; }
        public string NombreContratista { get; set; }
        public string NumeroPoliza { get; set; }
        public string NumeroActualizacion { get; set; }
        public string EstadoActualizacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? ContratoPolizaId { get; set; }
        public int ContratoPolizaActualizacionId { get; set; }
        public string NombreEstado { get; set; }
    }
}
