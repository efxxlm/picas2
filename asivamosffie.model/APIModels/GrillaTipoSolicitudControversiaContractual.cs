using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public  class GrillaTipoSolicitudControversiaContractual
    {
        public int ControversiaContractualId { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoControversiaCodigo { get; set; }
        public string TipoControversia { get; set; }
        public int ContratoId { get; set; }
     
    }
}
