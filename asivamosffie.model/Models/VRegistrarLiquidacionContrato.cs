using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarLiquidacionContrato
    {
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NombreEstado { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
    }
}
