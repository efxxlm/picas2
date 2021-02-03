using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalInterventoriaObservaciones
    {
        public int InformeFinalInterventoriaObservacionesId { get; set; }
        public int? InformeFinalInterventoriaId { get; set; }
        public string Observaciones { get; set; }
        public bool? EsSupervision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivado { get; set; }
        public bool? EsCalificacion { get; set; }

        public virtual InformeFinalInterventoria InformeFinalInterventoria { get; set; }
    }
}
