using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
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

        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
    }
}
