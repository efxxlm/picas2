using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFuentesUsoXcontratoIdXproyecto
    {
        public string NombreUso { get; set; }
        public string FuenteFinanciacion { get; set; }
        public string TipoUso { get; set; }
        public int? AportanteId { get; set; }
        public decimal ValorUso { get; set; }
        public int? ContratoId { get; set; }
        public int? ProyectoId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
    }
}
