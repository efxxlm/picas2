using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AportanteFuenteFinanciacion
    {
        public int AportanteFuenteFinanciacionId { get; set; }
        public int AportanteId { get; set; }
        public int FuenteFinanciacionId { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
    }
}
