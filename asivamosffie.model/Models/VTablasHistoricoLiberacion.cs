using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VTablasHistoricoLiberacion
    {
        public int? ComponenteUsoId { get; set; }
        public int? ComponenteUsoNovedadId { get; set; }
        public int? ComponenteUsoHistoricoId { get; set; }
        public int? ComponenteUsoNovedadHistoricoId { get; set; }
        public int? ContratacionProyectoAportanteId { get; set; }
        public int? ContratacionProyectoAportanteHistoricoId { get; set; }
        public int? NovedadContractualAportanteId { get; set; }
        public int? NovedadContractualAportanteHistoricoId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int? DisponibilidadPresupuestalHistoricoId { get; set; }
        public int? NovedadContractualRegistroPresupuestalId { get; set; }
        public int? NovedadContractualRegistroPresupuestalHistoricoId { get; set; }
        public int ContratoId { get; set; }
        public bool? EsNovedad { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int? ProyectoAportanteHistoricoId { get; set; }
        public int? ProyectoAportanteId { get; set; }
        public int GestionFuenteFinanciacionId { get; set; }
        public int? DisponibilidadPresupuestalProyectoId { get; set; }
        public int GestionFuenteFinanciacionHistoricoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorHistorico { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal NuevoSaldo { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotalAportante { get; set; }
        public decimal? ValorAporte { get; set; }
        public decimal ValorSolicitud { get; set; }
        public int FuenteFinanciacionId { get; set; }
    }
}
