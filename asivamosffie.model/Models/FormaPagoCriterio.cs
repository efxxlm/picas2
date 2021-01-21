using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FormaPagoCriterio
    {
        public int FormaPagoCriterioId { get; set; }
        public string FormaPagoCodigo { get; set; }
        public string CriterioPagoCodigo { get; set; }
        public bool? Eliminado { get; set; }
    }
}
