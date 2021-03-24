using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoContrato
    {
        public int ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NumeroDrp { get; set; }
        public decimal? ValorSolicitudDdp { get; set; }
        public decimal? SaldoPresupuestal { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public bool? SolicitudValidada { get; set; }
    }
}
