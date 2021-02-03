using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleTerceroCausacionDescuento
    {
        public int OrdenGiroDetalleTerceroCausacionDescuentoId { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionId { get; set; }
        public int? OrdenGiroDetalleDescuentoTecnicaAportanteId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public decimal? ValorDescuento { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual OrdenGiroDetalleDescuentoTecnicaAportante OrdenGiroDetalleDescuentoTecnicaAportante { get; set; }
        public virtual OrdenGiroDetalleTerceroCausacion OrdenGiroDetalleTerceroCausacion { get; set; }
    }
}
