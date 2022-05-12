using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoInfoContratacionProyecto
    {
        public string LlaveMen { get; set; }
        public int ProyectoId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string NumeroContratacion { get; set; }
        public string Contratista { get; set; }
        public string NumeroContrato { get; set; }
        public string InstitucionEducativa { get; set; }
        public string CodigoDaneInstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string CodigoDaneSede { get; set; }
        public string TipoIntervencion { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string UbicacionLatitud { get; set; }
        public string UbicacionLongitud { get; set; }
        public int? PlazoMesesObra { get; set; }
        public int? PlazoDiasObra { get; set; }
        public decimal? ValorObra { get; set; }
        public int? PlazoMesesInterventoria { get; set; }
        public int? PlazoDiasInterventoria { get; set; }
        public decimal? ValorInterventoria { get; set; }
    }
}
