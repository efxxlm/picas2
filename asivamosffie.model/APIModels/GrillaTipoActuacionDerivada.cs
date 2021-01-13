using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaTipoActuacionDerivada
    {
   
        public int ControversiaActuacionId { get; set; }
        public int SeguimientoActuacionDerivadaId { get; set; }        
        public string FechaActualizacion { get; set; }
        //public string FechaActuacion { get; set; }
        public string Actuacion { get; set; }
        public string NumeroActuacion { get; set; }
        
        public string EstadoRegistroActuacionDerivada { get; set; }
        public string EstadoActuacionDerivada { get; set; }
        public string EstadoActuacionDerivadaCodigo { get; set; }
    }
   
}

