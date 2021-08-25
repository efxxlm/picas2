using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAportanteFuente
    {
        public int ContratoId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public decimal? Valor { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public string FuenteRecursos { get; set; }
    }
}
