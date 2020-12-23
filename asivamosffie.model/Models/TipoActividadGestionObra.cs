using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TipoActividadGestionObra
    {
        public int TipoActividadGestionObraId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public string TipoActividadCodigo { get; set; }

        public virtual SeguimientoSemanalGestionObra SeguimientoSemanalGestionObra { get; set; }
    }
}
