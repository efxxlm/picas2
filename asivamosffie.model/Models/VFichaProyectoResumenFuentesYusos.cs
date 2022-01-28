using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoResumenFuentesYusos
    {
        public int? ContratacionProyectoAportanteId { get; set; }
        public string NombreAportante { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoContrato { get; set; }
        public string Fase { get; set; }
        public string Uso { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string Fuente { get; set; }
        public decimal ValorUso { get; set; }
    }
}
