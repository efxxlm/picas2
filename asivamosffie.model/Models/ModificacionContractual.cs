using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ModificacionContractual
    {
        public int ModificacionContractualId { get; set; }
        public int? NovedadContractualId { get; set; }
        public DateTime? FechaTramite { get; set; }
        public DateTime? FechaEnvioTramite { get; set; }
        public string Observaciones { get; set; }
        public string UrlMinuta { get; set; }

        public virtual NovedadContractual ModificacionContractualNavigation { get; set; }
    }
}
