using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaActaInicio
    { 
        public int ContratoId { get; set; }
        public string FechaAprobacionRequisitos { get; set; }
        public DateTime? FechaAprobacionRequisitosDate { get; set; }
        public string NumeroContratoObra { get; set; }
        public string EstadoActaCodigo { get; set; }
        public string EstadoActa { get; set; }
        public string EstadoVerificacion { get; set; }
        public string TipoContrato { get; set; }
        public string TipoContratoNombre { get; set; }
        public bool TieneObservacionesSupervisor { get; set; }
    }
}


