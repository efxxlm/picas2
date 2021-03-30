using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleTerceroCausacionAportante
    {
        public int OrdenGiroDetalleTerceroCausacionAportanteId { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionId { get; set; }
        public string FuenteRecursoCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? AportanteId { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public decimal? ValorDescuento { get; set; }
        public int? FuenteFinanciacionId { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual OrdenGiroDetalleTerceroCausacion OrdenGiroDetalleTerceroCausacion { get; set; }
    }
}
