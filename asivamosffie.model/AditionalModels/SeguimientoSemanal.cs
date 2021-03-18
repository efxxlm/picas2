using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanal
    {
         
        [NotMapped]
        public dynamic AvanceAcumulado { get; set; }     

        [NotMapped]
        public string ComiteObraGenerado { get; set; }

        [NotMapped]
        public int CantidadTotalDiasActividades { get; set; }

        [NotMapped]
        public List<Programacion> ListProgramacion { get; set; }

        [NotMapped]
        public List<dynamic> PeriodoReporteMensualFinanciero { get; set; }

        [NotMapped]
        public dynamic TablaFinanciera { get; set; }

        [NotMapped]
        public dynamic InfoProyecto { get; set; }
    }
}
