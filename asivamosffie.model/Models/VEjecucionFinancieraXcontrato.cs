using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VEjecucionFinancieraXcontrato
    {
        public int ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public decimal? ValorSolicitudDdp { get; set; }
        public decimal? ValorNeto { get; set; }
        public decimal? SaldoTesoral { get; set; }
        public bool? SolicitudValidada { get; set; }
    }
}
