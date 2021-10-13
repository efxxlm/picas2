using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class GestionFuenteFinanciacionHistorico
    {
        public int GestionFuenteFinanciacionHistoricoId { get; set; }
        public int GestionFuenteFinanciacionId { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal NuevoSaldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public decimal? SaldoActualGenerado { get; set; }
        public decimal? ValorSolicitadoGenerado { get; set; }
        public decimal? NuevoSaldoGenerado { get; set; }

        public virtual GestionFuenteFinanciacion GestionFuenteFinanciacion { get; set; }
    }
}
