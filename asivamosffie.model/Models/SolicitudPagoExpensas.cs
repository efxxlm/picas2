using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoExpensas
    {
        public int SolicitudPagoExpensasId { get; set; }
        public int SolicitudPagoId { get; set; }
        public int? NumeroRadicadoSac { get; set; }
        public int? NumeroFactura { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string TipoPagoCodigo { get; set; }
        public string ConceptoPagoCriterioCodigo { get; set; }
        public string ValorFacturadoConcepto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
