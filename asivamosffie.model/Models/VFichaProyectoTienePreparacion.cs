using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoTienePreparacion
    {
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ContratoId { get; set; }
    }
}
