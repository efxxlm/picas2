using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProyectosCierre
    {
        public DateTime? FechaTerminacionProyecto { get; set; }
        public string LlaveMen { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string SedeEducativa { get; set; }
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoInforme { get; set; }
    }
}
