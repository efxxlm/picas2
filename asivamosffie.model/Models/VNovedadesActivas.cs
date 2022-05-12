using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VNovedadesActivas
    {
        public int NovedadContractualId { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public int? ContratoId { get; set; }
        public int? ProyectoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? EsAplicadaAcontrato { get; set; }
        public string EstadoCodigo { get; set; }
    }
}
