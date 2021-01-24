using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{          
        
       public class GrillaNovedadTipo
    {        
        public int NovedadContractualId { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoNovedad { get; set; }
        public string EstadoNovedad { get; set; }
        public string EstadoRegistro { get; set; }
    }
}
