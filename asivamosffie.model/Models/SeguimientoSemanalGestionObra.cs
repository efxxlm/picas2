using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObra
    {
        public int SeguimientoSemanalGestionObraId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public bool? SeEjecutoGestionAmbiental { get; set; }
        public string TipoActividadCodigo { get; set; }
        public string Proveedor { get; set; }
        public bool? RequierePermisos { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
