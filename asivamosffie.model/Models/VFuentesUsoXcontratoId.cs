using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFuentesUsoXcontratoId
    {
        public string TipoUsoCodigo { get; set; }
        public decimal ValorUso { get; set; }
        public int? ContratoId { get; set; }
        public string NombreUso { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string FuenteFinanciacion { get; set; }
        public int? AportanteFuente { get; set; }
        public int? AportanteProyecto { get; set; }
    }
}
