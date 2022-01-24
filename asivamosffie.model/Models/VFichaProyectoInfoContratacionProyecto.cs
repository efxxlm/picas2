using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoInfoContratacionProyecto
    {
        public string LlaveMen { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public string NumeroContratacion { get; set; }
        public string Contratista { get; set; }
        public string NumeroContrato { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string TipoIntervencion { get; set; }
    }
}
