using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualObservaciones
    {
        public int NovedadContractualObservacionesId { get; set; }
        public int? NovedadContractualId { get; set; }
        public string Observaciones { get; set; }
        public bool? EsSupervision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivado { get; set; }

        public virtual NovedadContractual NovedadContractual { get; set; }
    }
}
