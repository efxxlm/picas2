using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFacturadoContratoXproyecto
    {
        public int ContratacionId { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public decimal? ValorSolicitudDdp { get; set; }
        public decimal? ValorFacturado { get; set; }
        public decimal? SaldoPresupuestal { get; set; }
        public bool EsPreconstruccion { get; set; }
        public bool? SolicitudValidada { get; set; }
    }
}
