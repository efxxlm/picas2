using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaContractual
    {
<<<<<<< HEAD
        public ControversiaContractual()
        {
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
        }

=======
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
        public int ControversiaContractualId { get; set; }
        public string TipoControversiaCodigo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoCodigo { get; set; }
        public bool EsCompleto { get; set; }
<<<<<<< HEAD

        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
=======
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
    }
}
