using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinancieroTrasladoValor
    {
        public int BalanceFinancieroTrasladoValorId { get; set; }
        public int BalanceFinancieroTrasladoId { get; set; }
        public string TipoTrasladoCodigo { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionAportanteId { get; set; }
        public int? OrdenGiroDetalleTerceroCausacionDescuentoId { get; set; }
        public decimal? ValorTraslado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroDetalleDescuentoTecnicaId { get; set; }
        public int? OrdenGiroDetalleDescuentoTecnicaAportanteId { get; set; }
        public int? OrdenGiroId { get; set; }
        public int? BalanceFinancieroId { get; set; }

        public virtual BalanceFinanciero BalanceFinanciero { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual OrdenGiroDetalleDescuentoTecnica OrdenGiroDetalleDescuentoTecnica { get; set; }
        public virtual OrdenGiroDetalleDescuentoTecnicaAportante OrdenGiroDetalleDescuentoTecnicaAportante { get; set; }
        public virtual OrdenGiroDetalleTerceroCausacionAportante OrdenGiroDetalleTerceroCausacionAportante { get; set; }
        public virtual OrdenGiroDetalleTerceroCausacionDescuento OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
    }
}
