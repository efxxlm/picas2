using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObra
    {
        public SeguimientoSemanalGestionObra()
        {
            ProveedorGestionObra = new HashSet<ProveedorGestionObra>();
            TipoActividadGestionObra = new HashSet<TipoActividadGestionObra>();
        }

        public int SeguimientoSemanalGestionObraId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public bool? SeEjecutoGestionAmbiental { get; set; }
        public bool? RegistroCompletoGestionAmbiental { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
        public virtual ICollection<ProveedorGestionObra> ProveedorGestionObra { get; set; }
        public virtual ICollection<TipoActividadGestionObra> TipoActividadGestionObra { get; set; }
    }
}
