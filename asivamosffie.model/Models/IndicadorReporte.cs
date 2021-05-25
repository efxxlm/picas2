using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class IndicadorReporte
    {
        public int IndicadorReporteId { get; set; }
        public string Nombre { get; set; }
        public string ReportId { get; set; }
        public string GroupId { get; set; }
        public bool Indicador { get; set; }
        public string Etapa { get; set; }
        public string Proceso { get; set; }
        public bool Activo { get; set; }
    }
}
