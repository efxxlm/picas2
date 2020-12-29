using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
   public class GrillaProcesoDefensaJudicial
    {
        public bool? VaAProcesoJudicial { get; set; }

        public int DefensaJudicialId { get; set; }
        public string FechaRegistro { get; set; }
        public string LegitimacionPasivaActiva { get; set; }
        public string NumeroProceso { get; set; }
        public string TipoAccion { get; set; }
        public string TipoAccionCodigo { get; set; }
        public string EstadoProceso { get; set; }
        public string EstadoProcesoCodigo { get; set; }
        //public string RegistroCompletoCodigo { get; set; }
        public string RegistroCompletoNombre { get; set; }
        public string TipoProceso { get; set; }
        public string TipoProcesoCodigo { get; set; }


    }
}
