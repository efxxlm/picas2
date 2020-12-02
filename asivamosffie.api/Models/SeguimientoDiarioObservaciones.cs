using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SeguimientoDiarioObservaciones
    {
        public SeguimientoDiarioObservaciones()
        {
            SeguimientoDiario = new HashSet<SeguimientoDiario>();
        }

        public int SeguimientoDiarioObservacionesId { get; set; }
        public int? SeguimientoDiarioId { get; set; }
        public string Observaciones { get; set; }
        public bool? EsSupervision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivado { get; set; }

        public virtual SeguimientoDiario SeguimientoDiarioNavigation { get; set; }
        public virtual ICollection<SeguimientoDiario> SeguimientoDiario { get; set; }
    }
}
