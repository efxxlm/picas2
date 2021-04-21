using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VComponenteUsoNovedad
    {
        public int NovedadContractualId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public decimal? ValorAporte { get; set; }
        public string TipoComponenteCodigo { get; set; }
        public string FaseCodigo { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string TipoUsoCodigo { get; set; }
        public decimal ValorUso { get; set; }
    }
}
