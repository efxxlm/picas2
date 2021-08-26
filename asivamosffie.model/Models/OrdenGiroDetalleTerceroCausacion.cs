using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleTerceroCausacion
    {
        public OrdenGiroDetalleTerceroCausacion()
        {
            OrdenGiroDetalleTerceroCausacionAportante = new HashSet<OrdenGiroDetalleTerceroCausacionAportante>();
            OrdenGiroDetalleTerceroCausacionDescuento = new HashSet<OrdenGiroDetalleTerceroCausacionDescuento>();
        }

        public int OrdenGiroDetalleTerceroCausacionId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public decimal? ValorNetoGiro { get; set; }
        public int? OrdenGiroDetalleId { get; set; }
        public string ConceptoPagoCriterio { get; set; }
        public string TipoPagoCodigo { get; set; }
        public bool? TieneDescuento { get; set; }
        public bool? RegistroCompletoOrigen { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string ConceptoCodigo { get; set; }
        public decimal ValorFacturadoConcepto { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionAportante> OrdenGiroDetalleTerceroCausacionAportante { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionDescuento> OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
    }
}
