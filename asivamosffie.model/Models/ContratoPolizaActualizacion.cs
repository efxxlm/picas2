using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPolizaActualizacion
    {
        public int ContratoPolizaActualizacionId { get; set; }
        public int? ContratoPolizaId { get; set; }
        public string RazonActualizacionCodigo { get; set; }
        public string TipoActualizacionCodigo { get; set; }
        public DateTime? FechaExpedicionActualizacionPoliza { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
    }
}
