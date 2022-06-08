using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOdgPagos
    {
        public int OrdenGiroId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string TipoPagoCodigo { get; set; }
        public int? AportanteId { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public string CriterioPagoCodigo { get; set; }
    }
}
