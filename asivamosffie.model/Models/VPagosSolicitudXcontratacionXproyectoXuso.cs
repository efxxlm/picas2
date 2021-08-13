using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VPagosSolicitudXcontratacionXproyectoXuso
    {
        public int? ContratacionProyectoId { get; set; }
        public string NumeroDrp { get; set; }
        public string TipoUsoCodigo { get; set; }
        public string Uso { get; set; }
        public decimal? SaldoUso { get; set; }
    }
}
