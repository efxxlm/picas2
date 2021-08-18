using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VContratosXcontratacionProyecto
    {
        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ContratacionProyectoId { get; set; }
    }
}
