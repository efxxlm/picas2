using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VCompromisoSeguimiento
    {
        public int CompromisoSeguimientoId { get; set; }
        public string DescripcionSeguimiento { get; set; }
        public bool? Eliminado { get; set; }
        public int? SesionParticipanteId { get; set; }
        public string EstadoCompromisoCodigo { get; set; }
        public int? SesionSolicitudCompromisoId { get; set; }
    }
}
