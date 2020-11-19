using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class ListCompromisos
    {
        public int CompromisoId { get; set; }
        
        public int ComiteTecnicoId { get; set; }

        public DateTime?  FechaComite { get; set; }

        public string NumeroComite { get; set; }

        public string Compromiso { get; set; }

        public string EstadoCodigo { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string TipoSolicitud { get; set; }

        public string FechaCumplimiento { get; set; }

        public List<dynamic> Seguimiento { get; set; }
    }
}
