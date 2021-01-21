using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObra
    {
        public SeguimientoSemanalGestionObra()
        {
            SeguimientoSemanalGestionObraAlerta = new HashSet<SeguimientoSemanalGestionObraAlerta>();
            SeguimientoSemanalGestionObraAmbiental = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
            SeguimientoSemanalGestionObraCalidad = new HashSet<SeguimientoSemanalGestionObraCalidad>();
            SeguimientoSemanalGestionObraSeguridadSalud = new HashSet<SeguimientoSemanalGestionObraSeguridadSalud>();
            SeguimientoSemanalGestionObraSocial = new HashSet<SeguimientoSemanalGestionObraSocial>();
            TipoActividadGestionObra = new HashSet<TipoActividadGestionObra>();
        }

        public int SeguimientoSemanalGestionObraId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAlerta> SeguimientoSemanalGestionObraAlerta { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbiental { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraCalidad> SeguimientoSemanalGestionObraCalidad { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSeguridadSalud> SeguimientoSemanalGestionObraSeguridadSalud { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSocial> SeguimientoSemanalGestionObraSocial { get; set; }
        public virtual ICollection<TipoActividadGestionObra> TipoActividadGestionObra { get; set; }
    }
}
