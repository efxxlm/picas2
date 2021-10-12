using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VConceptosXcontratoXfaseXproyecto
    {
        public int? ContratoId { get; set; }
        public bool EsPreconstruccion { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public string ConceptoCodigo { get; set; }
        public string ConceptoNombre { get; set; }
    }
}
