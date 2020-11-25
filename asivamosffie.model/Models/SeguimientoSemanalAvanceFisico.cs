using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalAvanceFisico
    {
        public int SeguimientoSemanalAvanceFisicoId { get; set; }
        public int SeguimientoSemanalId { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
