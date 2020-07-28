using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class PolizaObservacion
    {
        public int PolizaObservacionId { get; set; }
        public int ContratoPolizaId { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaRevision { get; set; }
        public string EstadoRevisionCodigo { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
    }
}
