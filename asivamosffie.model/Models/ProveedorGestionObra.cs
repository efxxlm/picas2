using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProveedorGestionObra
    {
        public int ProveedorGestionObraId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public string Proveedor { get; set; }
        public bool? RequierePermisos { get; set; }
        public string Url { get; set; }
        public bool Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SeguimientoSemanalGestionObra SeguimientoSemanalGestionObra { get; set; }
    }
}
