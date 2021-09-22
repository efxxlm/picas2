using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleDescuentoTecnicaAportante
    {
        public OrdenGiroDetalleDescuentoTecnicaAportante()
        {
            BalanceFinancieroTrasladoValor = new HashSet<BalanceFinancieroTrasladoValor>();
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
        public string FuenteRecursosCodigo { get; set; }
        public int? OrdenGiroDetalleDescuentoTecnicaId { get; set; }
        public int? FuenteFinanciacionId { get; set; }

        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual OrdenGiroDetalleDescuentoTecnica OrdenGiroDetalleDescuentoTecnica { get; set; }
        public virtual ICollection<BalanceFinancieroTrasladoValor> BalanceFinancieroTrasladoValor { get; set; }
    }
}
