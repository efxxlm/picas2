using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class GestionFuenteFinanciacion
    {
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

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual DisponibilidadPresupuestalProyecto DisponibilidadPresupuestalProyecto { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
    }
}
