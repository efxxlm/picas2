using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroSoporte
    {
        public OrdenGiroSoporte()
        {
            OrdenGiroDetalle = new HashSet<OrdenGiroDetalle>();
        }

        public int OrdenGiroSoporteId { get; set; }
        public string UrlSoporte { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
    }
}
