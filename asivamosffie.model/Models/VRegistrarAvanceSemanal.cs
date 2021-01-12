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
        public bool? RegistroCompleto { get; set; }
        public string FechaUltimoReporte { get; set; }
        public string EstadoObra { get; set; }
        public int? CantidadSemanas { get; set; }
        public int? NumeroSemana { get; set; }
        public bool? ActaCargada { get; set; }
    }
}
