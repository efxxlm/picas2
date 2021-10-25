using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPago
    {

        [NotMapped]
        public List<VConceptosUsosXsolicitudPagoId> VConceptosUsosXsolicitudPagoId { get; set; }

        [NotMapped]
        public dynamic TablaDrpUso { get; set; }

        [NotMapped]
        public Contrato ContratoSon { get; set; }

        [NotMapped]
        public decimal? SaldoPresupuestal { get; set; }

        [NotMapped]
        public List<TablaDRP> TablaDRP { get; set; }

        [NotMapped]
        public TablaUsoFuenteAportante TablaUsoFuenteAportante { get; set; }

        [NotMapped]
        public dynamic TablaPorcentajeParticipacion { get; set; }

        [NotMapped]
        public dynamic TablaInformacionFuenteRecursos { get; set; }

        [NotMapped]
        public string MedioPagoCodigo { get; set; }

        [NotMapped]
        public OrdenGiroTerceroChequeGerencia PrimerOrdenGiroTerceroChequeGerencia { get; set; }

        [NotMapped]
        public OrdenGiroTerceroTransferenciaElectronica PrimerOrdenGiroTerceroTransferenciaElectronica { get; set; }
         
        [NotMapped]
        public dynamic ValorXProyectoXFaseXAportanteXConcepto { get; set; }

        [NotMapped]
        public List<VAmortizacionXproyecto> VAmortizacionXproyecto { get; set; }
    }

}
