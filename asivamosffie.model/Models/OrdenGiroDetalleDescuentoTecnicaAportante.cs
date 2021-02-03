using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleDescuentoTecnicaAportante
    {
        public OrdenGiroDetalleDescuentoTecnicaAportante()
        {
            OrdenGiroDetalleTerceroCausacionDescuento = new HashSet<OrdenGiroDetalleTerceroCausacionDescuento>();
        }

        public int OrdenGiroDetalleDescuentoTecnicaAportanteId { get; set; }
        public int SolicitudPagoFaseFacturaDescuentoId { get; set; }
        public int? AportanteId { get; set; }
        public int? ValorDescuento { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public bool? RequiereDescuento { get; set; }

        public virtual SolicitudPagoFaseFacturaDescuento SolicitudPagoFaseFacturaDescuento { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionDescuento> OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
    }
}
