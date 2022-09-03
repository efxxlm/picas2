using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VControlRecursos
    {
        public int? Vigencia { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public int? TipoAportanteId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string NombreCuentaBanco { get; set; }
        public string NumeroCuentaBanco { get; set; }
        public int? AportanteId { get; set; }
        public string NumeroRp { get; set; }
        public DateTime FechaConsignacion { get; set; }
        public decimal ValorConsignacion { get; set; }
        public int ControlRecursoId { get; set; }
    }
}
