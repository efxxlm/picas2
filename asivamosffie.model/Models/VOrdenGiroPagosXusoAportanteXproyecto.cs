using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOrdenGiroPagosXusoAportanteXproyecto
    {
        public int? ContratacionId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public int? OrdenGiroId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? AportanteId { get; set; }
        public int? ValorDescuento { get; set; }
        public string TipoUsoCodigo { get; set; }
    }
}
