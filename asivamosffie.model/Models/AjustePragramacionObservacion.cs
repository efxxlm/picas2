using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AjustePragramacionObservacion
    {
        public int AjustePragramacionObservacionId { get; set; }
        public int? AjusteProgramacionId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivada { get; set; }
        public bool? EsObra { get; set; }
        public int? ArchivoCargueId { get; set; }
        public bool? EsSupervisor { get; set; }

        public virtual AjusteProgramacion AjusteProgramacion { get; set; }
    }
}
