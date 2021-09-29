using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class Contrato
    {
        [NotMapped]
        public IFormFile PFile { get; set; }

        [NotMapped]
        public DateTime? FechaAprobacionComite { get; set; }

        [NotMapped]
        public DateTime? FechaEnvioFirmaFormat { get; set; }

        [NotMapped]
        public DateTime? FechaFirmaContratistaFormat { get; set; }

        [NotMapped]
        public DateTime? FechaFirmaFiduciariaFormat { get; set; }

        [NotMapped]
        public DateTime? FechaFirmaContratoFormat { get; set; }

        [NotMapped]
        public List<CofinanicacionAportanteGrilla> ListAportantes { get; set; }

        [NotMapped]
        public decimal ValorFase1 { get; set; }

        [NotMapped]
        public decimal ValorFase2 { get; set; }

        [NotMapped]
        public DateTime? FechaPolizaAprobacion { get; set; }

        [NotMapped]
        public Usuario UsuarioInterventoria { get; set; }

        [NotMapped]
        public bool TieneFase1 { get; set; }

        [NotMapped]
        public bool TieneFase2 { get; set; }

        [NotMapped]
        public SolicitudPago SolicitudPagoOnly { get; set; }

        [NotMapped]
        public dynamic ListProyectos { get; set; }

        [NotMapped]
        public dynamic MontoMaximo { get; set; }

        [NotMapped]
        public dynamic MontoPendiente { get; set; }

        [NotMapped]
        public List<VValorFacturadoContrato> ValorFacturadoContrato { get; set; }

        [NotMapped]
        public dynamic VContratoPagosRealizados { get; set; }

        [NotMapped]
        public bool TieneSuspensionAprobada { get; set; }

        [NotMapped]
        public int? SuspensionAprobadaId { get; set; }

        [NotMapped]
        public TablaUsoFuenteAportante TablaUsoFuenteAportante { get; set; }

        [NotMapped]
        public decimal? ValorPagadoContratista { get; set; }

        [NotMapped]
        public DateTime? FechaAprobacionPoliza { get; set; }

        [NotMapped]
        public bool TieneActa { get; set; }

        //[NotMapped]
        //public List<TablaDRP> TablaDRP { get; set; }

        [NotMapped]
        public dynamic TablaDRP { get; set; }

        [NotMapped]
        public dynamic TablaDRPODG { get; set; }
         
        [NotMapped]
        public DateTime? FechaEstimadaFinalizacion { get; set; }

        [NotMapped]
        public bool? EsMultiProyecto { get; set; }


        [NotMapped]
        public dynamic TablaRecursosComprometidos { get; set; }


        [NotMapped]
        public List<VAmortizacionXproyecto> VAmortizacionXproyecto{ get; set; }
    }


}
