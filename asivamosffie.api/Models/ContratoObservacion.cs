using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ContratoObservacion
    {
        public int ContratoObservacionId { get; set; }
        public int ContratoId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Contrato Contrato { get; set; }
    }
}
