using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
<<<<<<< HEAD
=======
        public DefensaJudicial()
        {
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
        }

>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
        public int DefensaJudicialId { get; set; }
        public string LegitimacionCodigo { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public string NumeroProceso { get; set; }
        public int CantContratos { get; set; }
        public string EstadoProcesoCodigo { get; set; }
<<<<<<< HEAD
        public int SolicitudId { get; set; }
=======

        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
    }
}
