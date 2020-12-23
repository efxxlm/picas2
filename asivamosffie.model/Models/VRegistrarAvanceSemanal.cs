using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarAvanceSemanal
    {
        public string TipoContrato { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroContrato { get; set; }
        public int ContratacionProyectoId { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string EstadoObraCodigo { get; set; }
        public int? NumeroSemana { get; set; }
        public string FechaUltimoReporte { get; set; }
        public string EstadoObra { get; set; }
        public bool? EnviarSupervisor { get; set; }
    }
}
