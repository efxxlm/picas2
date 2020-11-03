using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
   public class VistaContratoGarantiaPoliza
    {

        public int? IdContrato { get; set; }
        //tipo identificacion
        public string TipoContrato { get; set; }
        public string NumeroContrato { get; set; }   
        
        public string ObjetoContrato { get; set; }        

        public string NombreContratista { get; set; }
        
        public string NumeroIdentificacion { get; set; }

        public string TipoDocumento { get; set; }

        //Nit  ????

        public string ValorContrato { get; set; }
        
        public string PlazoContrato { get; set; }

        public string EstadoRegistro { get; set; }        

        public bool? RegistroCompleto { get; set; }

        public string? TipoSolicitud { get; set; }        

        public string? DescripcionModificacion { get; set; }

        public string? TipoModificacion { get; set; }


    }
}
