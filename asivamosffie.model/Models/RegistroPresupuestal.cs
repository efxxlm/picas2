using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class RegistroPresupuestal
    {
        public RegistroPresupuestal()
        {
            ControlRecurso = new HashSet<ControlRecurso>();
        }

        public int RegistroPresupuestalId { get; set; }
        public int AportanteId { get; set; }
        public string NumeroRp { get; set; }
        public DateTime FechaRp { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Aportante Aportante { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
    }
}
