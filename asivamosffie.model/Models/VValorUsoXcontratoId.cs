using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorUsoXcontratoId
    {
        public string TipoUsoCodigo { get; set; }
        public decimal? ValorUso { get; set; }
        public int ContratoId { get; set; }
        public string Nombre { get; set; }
        public bool? EsPreConstruccion { get; set; }
        public string FaseId { get; set; }
        public int ContratacionProyectoId { get; set; }
    }
}
