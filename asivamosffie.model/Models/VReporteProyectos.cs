using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VReporteProyectos
    {
        public int ProyectoId { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public string InstitucionEducativa { get; set; }
        public string TipoProyecto { get; set; }
        public string EstadoProyectoObra { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
