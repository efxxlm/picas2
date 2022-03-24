using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorContratoNovedad
    {
        public int? ContratacionId { get; set; }
        public decimal ValorContrato { get; set; }
        public decimal? ValorAdicionalContrato { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int ContratoId { get; set; }
    }
}
