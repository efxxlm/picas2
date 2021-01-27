using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleTerceroCausacion
    {
        public OrdenGiroDetalleTerceroCausacion()
        {
            OrdenGiroDetalle = new HashSet<OrdenGiroDetalle>();
            OrdenGiroDetalleTerceroCausacionDescuento = new HashSet<OrdenGiroDetalleTerceroCausacionDescuento>();
        }

        public int OrdenGiroDetalleTerceroCausacionId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual ICollection<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionDescuento> OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
    }
}
