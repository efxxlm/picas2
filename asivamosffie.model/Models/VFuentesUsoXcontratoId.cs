using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFuentesUsoXcontratoId
    {
        public string NombreUso { get; set; }
        public string FuenteFinanciacion { get; set; }
        public string TipoUso { get; set; }
        public int? ContratoId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
    }
}
