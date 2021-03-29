using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorUsoXcontratoAportante
    {
        public string TipoUsoCodigo { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public decimal ValorUso { get; set; }
        public int ContratoId { get; set; }
        public int AportanteId { get; set; }
        public int SolicitudPagoId { get; set; }
        public bool? EsPreConstruccion { get; set; }
        public string FaseId { get; set; }
    }
}
