using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAportanteFuenteUso
    {
        public int ContratoId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public decimal? ValorUso { get; set; }
        public string TipoUsoCodigo { get; set; }
        public string Nombre { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
    }
}
