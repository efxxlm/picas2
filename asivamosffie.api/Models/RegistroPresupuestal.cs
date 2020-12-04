using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class RegistroPresupuestal
    {
        public RegistroPresupuestal()
        {
            ControlRecurso = new HashSet<ControlRecurso>();
        }

        public int RegistroPresupuestalId { get; set; }
        public int? AportanteId { get; set; }
        public string NumeroRp { get; set; }
        public DateTime? FechaRp { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? CofinanciacionDocumentoId { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual CofinanciacionDocumento CofinanciacionDocumento { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
    }
}
