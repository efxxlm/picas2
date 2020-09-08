using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
<<<<<<< HEAD
        public DefensaJudicial()
        {
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
        }

=======
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
        public int DefensaJudicialId { get; set; }
        public string LegitimacionCodigo { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public string NumeroProceso { get; set; }
        public int CantContratos { get; set; }
        public string EstadoProcesoCodigo { get; set; }
<<<<<<< HEAD

        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
=======
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
    }
}
