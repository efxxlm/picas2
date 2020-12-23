using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ManejoResiduosPeligrososEspeciales
    {
        public ManejoResiduosPeligrososEspeciales()
        {
            SeguimientoSemanalGestionObraAmbiental = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
        }

        public int ManejoResiduosPeligrososEspecialesId { get; set; }
        public bool? EstanClasificados { get; set; }
        public bool? RequiereObservacion { get; set; }
        public string Observacion { get; set; }
        public string UrlRegistroFotografico { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbiental { get; set; }
    }
}
