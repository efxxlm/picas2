using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ContratacionObservacion
    {
        public int ContratacionObservacionId { get; set; }
        public int ContratacionId { get; set; }
        public string Observacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? ComiteTecnicoId { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Contratacion Contratacion { get; set; }
    }
}
