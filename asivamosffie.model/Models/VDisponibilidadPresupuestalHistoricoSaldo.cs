using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDisponibilidadPresupuestalHistoricoSaldo
    {
        public DateTime FechaSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public string TipoSolicitudEspecialCodigo { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public string NumeroSolicitud { get; set; }
        public string NumeroDdp { get; set; }
        public int? ContratacionId { get; set; }
        public int? NovedadContractualRegistroPresupuestalId { get; set; }
        public bool? EsNovedad { get; set; }
        public int? NovedadContractualId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroContrato { get; set; }
    }
}
