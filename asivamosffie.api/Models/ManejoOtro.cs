using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ManejoOtro
    {
        public ManejoOtro()
        {
            SeguimientoSemanalGestionObraAmbiental = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
        }

        public int ManejoOtroId { get; set; }
        public DateTime? FechaActividad { get; set; }
        public string Actividad { get; set; }
        public string UrlSoporteGestion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbiental { get; set; }
    }
}
