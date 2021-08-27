using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDescuentosXordenGiroXproyectoXaportanteXconcepto
    {
        public int? OrdenGiroId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public string Nombre { get; set; }
        public int? ConceptoCodigo { get; set; }
        public int? AportanteId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public decimal ValorDescuento { get; set; }
        public string NumeroContrato { get; set; }
    }
}
