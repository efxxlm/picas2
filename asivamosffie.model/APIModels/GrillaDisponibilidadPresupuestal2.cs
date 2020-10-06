using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaDisponibilidadPresupuestal
    {
        public int DisponibilidadPresupuestalId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitudText { get; set; }
        public string NumeroSolicitud { get; set; }
        public string OpcionPorContratarCodigo { get; set; }
        public string OpcionPorContratarText{ get; set; }
        public decimal ValorSolicitado { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public string EstadoSolicitudText { get; set; }
        public string EstadoRegistro { get; set; }

    }
}
