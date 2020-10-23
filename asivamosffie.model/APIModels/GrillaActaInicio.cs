using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaActaInicio
    {

        public int ContratoId { get; set; }                      
        public string FechaAprobacionRequisitos { get; set; }
        public string NumeroContratoObra { get; set; }
        public string  EstadoActa { get; set; }
        public string TipoContrato { get; set; }
    }
}


