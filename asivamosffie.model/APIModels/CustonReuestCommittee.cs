using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class CustonReuestCommittee
    {

        public int ContratacionId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public DateTime FechaSolicitud { get; set; }

        public string TipoSolicitudText { get; set; }

        public int TipoSolicitudCodigo { get; set; }        
        public string NumeroSolicitud { get; set; }
        public string OpcionContratar { get; set; }
        public decimal ValorSolicitud { get; set; }        
        public DateTime FechaComite { get; set; }
        public decimal ValorSolicitado { get; set; }
		public int TipoAportanteId{ get; set; }        
        public string EstadoSolicitudCodigo { get; set; }
        public string EstadoSolicitudText { get; set; }
        public int NovedadContractualId { get; set; }


    }
}
