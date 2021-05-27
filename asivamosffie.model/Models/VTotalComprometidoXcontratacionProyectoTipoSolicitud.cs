using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VTotalComprometidoXcontratacionProyectoTipoSolicitud
    {
        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ProyectoId { get; set; }
        public decimal? TotalComprometido { get; set; }
    }
}
