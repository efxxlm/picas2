using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class FormaPagoCodigoCriterioPagoCodigo
    {
        public int FormaPagoCodigoCriterioPagoCodigoId { get; set; }
        public string FormaPagoCodigo { get; set; }
        public string CriterioPagoCodigo { get; set; }
        public bool? Eliminado { get; set; }
    }
}
