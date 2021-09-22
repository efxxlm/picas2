using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanal
    {
        [NotMapped]
        public List<InformacionGeneralProyecto> InformacionGeneralProyecto { get; set; }

        [NotMapped]
        public List<dynamic> InformacionGeneral { get; set; }
         
        [NotMapped]
        public List<dynamic> AvanceFisico { get; set; }
         
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


    public partial class InformacionGeneralProyecto
    {
        [NotMapped]
        public int SemanaNo { get; set; }

        [NotMapped]
        public DateTime? SemanaInicio { get; set; }

        [NotMapped]
        public DateTime? SemanaFin { get; set; }

        [NotMapped]
        public string LocalizacionProyecto { get; set; }

        [NotMapped]
        public VUbicacionXproyecto Ubicacion { get; set; } 
    }
}
