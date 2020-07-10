using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoAdministrativoAportante
    {
        public int ProyectoAdministrativoAportanteId { get; set; }
        public int ProyectoAdminstrativoId { get; set; }
        public int AportanteId { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual ProyectoAdministrativo ProyectoAdminstrativo { get; set; }
    }
}
