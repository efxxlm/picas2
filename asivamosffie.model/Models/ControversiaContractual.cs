using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaContractual
    {
<<<<<<< HEAD
=======
        public ControversiaContractual()
        {
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
        }

>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
        public int ControversiaContractualId { get; set; }
        public string TipoControversiaCodigo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoCodigo { get; set; }
        public bool EsCompleto { get; set; }
<<<<<<< HEAD
        public int SolicitudId { get; set; }
=======

        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
    }
}
