using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionObservacion
    {
        public int ConstruccionObservacionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string TipoObservacionConstruccion { get; set; }
        public string Observaciones { get; set; }
        public bool? EsSupervision { get; set; }
        public bool? EsActa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
    }
}
