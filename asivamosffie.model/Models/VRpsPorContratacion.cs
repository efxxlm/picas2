using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRpsPorContratacion
    {
        public string NumeroDrp { get; set; }
        public decimal ValorSolicitud { get; set; }
        public int? DisponibilidadPresupuestalId { get; set; }
        public int? ContratacionId { get; set; }
        public bool? EsNovedad { get; set; }
        public int? NovedadContractualId { get; set; }
    }
}
