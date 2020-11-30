using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class NotificacionMensajeGestionPoliza
    {
        public string NombreAseguradora { get; set; }
        public string NumeroPoliza { get; set; } 
        public string FechaRevision { get; set; }
        public DateTime? FechaRevisionDateTime { get; set; }
        public string FechaAprobacion { get; set; }
        public string  EstadoRevision { get; set; } 
        public string Observaciones { get; set; }

        public string? NumeroDRP { get; set; }


    }
}
