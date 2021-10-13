using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class GestionFuenteFinanciacion
    {
        public GestionFuenteFinanciacion()
        {
            GestionFuenteFinanciacionHistorico = new HashSet<GestionFuenteFinanciacionHistorico>();
        }

        public int GestionFuenteFinanciacionId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal NuevoSaldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? DisponibilidadPresupuestalProyectoId { get; set; }
        public string EstadoCodigo { get; set; }
        public int? DisponibilidadPresupuestalId { get; set; }
        public bool? EsNovedad { get; set; }
        public int? NovedadContractualRegistroPresupuestalId { get; set; }
        public int? RendimientosIncorporadosId { get; set; }
        public decimal? SaldoActualGenerado { get; set; }
        public decimal? ValorSolicitadoGenerado { get; set; }
        public decimal? NuevoSaldoGenerado { get; set; }
        public int? BalanceFinancieroTrasladoValorId { get; set; }

        public virtual BalanceFinancieroTrasladoValor BalanceFinancieroTrasladoValor { get; set; }
        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual DisponibilidadPresupuestalProyecto DisponibilidadPresupuestalProyecto { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual RendimientosIncorporados RendimientosIncorporados { get; set; }
        public virtual ICollection<GestionFuenteFinanciacionHistorico> GestionFuenteFinanciacionHistorico { get; set; }
    }
}
