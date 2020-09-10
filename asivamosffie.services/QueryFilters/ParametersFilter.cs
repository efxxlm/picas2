using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.QueryFilters
{
    public class ParametersFilter
    {
        public int requestId { get; set; }
        public string newStatus { get; set; } // Nuevo estado para disponibilidad presupiestal
        public string observation { get; set; }
        public string user { get; set; }

    }
}
