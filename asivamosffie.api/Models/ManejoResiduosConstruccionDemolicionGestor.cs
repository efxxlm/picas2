using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ManejoResiduosConstruccionDemolicionGestor
    {
        public int ManejoResiduosConstruccionDemolicionGestorId { get; set; }
        public int? ManejoResiduosConstruccionDemolicionId { get; set; }
        public string NombreGestorResiduos { get; set; }
        public bool? TienePermisoAmbiental { get; set; }
        public string Url { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ManejoResiduosConstruccionDemolicion ManejoResiduosConstruccionDemolicion { get; set; }
    }
}
