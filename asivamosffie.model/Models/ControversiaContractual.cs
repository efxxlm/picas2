using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaContractual
    {
        public int ControversiaContractualId { get; set; }
        public string TipoControversiaCodigo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoCodigo { get; set; }
        public bool EsCompleto { get; set; }
        public int SolicitudId { get; set; }
    }
}
