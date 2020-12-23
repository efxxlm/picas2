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
        public string EstadoControversia { get; set; }
        public int ContratoId { get; set; }
        public string RegistroCompletoNombre { get; set; }

        //cu 4.4.1
        public string? Actuacion { get; set; }
        public string? FechaActuacion { get; set; }
        public string? EstadoActuacion { get; set; }
        public string EstadoControversiaCodigo { get; set; }
    }
}
