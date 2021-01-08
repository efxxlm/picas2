using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
   public class GrillaActuacionSeguimiento
    {
        public int ActuacionSeguimientoId { get; set; }
        public string FechaActualizacion { get; set; }
        public string Actuacion { get; set; }
        public string NumeroActuacion { get; set; }
        public string NumeroReclamacion { get; set; }
        public string EstadoReclamacion { get; set; }
        public string NumeroActuacionFormat { get; set; }
        public string EstadoReclamacionCodigo { get; set; }
        public int ControversiaActuacionId { get; set; }
        public string EstadoActuacion { get; set; }
        public string EstadoActuacionCodigo { get; set; }
        public string EstadoRegistro { get; set; }
    }
}
