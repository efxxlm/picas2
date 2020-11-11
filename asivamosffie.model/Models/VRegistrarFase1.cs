using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarFase1
    {
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int? CantidadPerfilesAsociados { get; set; }
        public int? CantidadPerfilesAprobados { get; set; }
        public int? CantidadPerfilesPendientes { get; set; }
        public int? EstaDevuelto { get; set; }
        public int RegistroCompleto { get; set; }
    }
}
