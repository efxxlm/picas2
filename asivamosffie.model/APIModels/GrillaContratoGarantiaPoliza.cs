using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaContratoGarantiaPoliza
    {

        //idContratoPoliza
        //    idContrato        
        public int ContratoId { get; set; }
        public string  FechaFirma { get; set; }
        public string NumeroContrato { get; set; }          
        public string TipoSolicitud   { get; set; }
        public string TipoSolicitudCodigo { get; set; }

        public string EstadoPoliza { get; set; }
        public string EstadoPolizaCodigo { get; set; }
        public string EstadoRegistro { get; set; }

        public bool? RegistroCompleto { get; set; }

        public string RegistroCompletoNombre { get; set; }




    }
}
