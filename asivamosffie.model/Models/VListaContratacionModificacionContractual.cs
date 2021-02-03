using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VListaContratacionModificacionContractual
    {
        public string TipoSolicitud { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public string NumeroDdp { get; set; }
        public string EstadoCodigo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public int SolicitudId { get; set; }
        public bool? EstadoRegistro { get; set; }
        public string EstadoDelRegistro { get; set; }
        public bool? EstaTramitado { get; set; }
    }
}
