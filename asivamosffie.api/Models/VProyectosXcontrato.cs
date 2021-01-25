using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class VProyectosXcontrato
    {
        public string LlaveMen { get; set; }
        public DateTime? FechaRegistroProyecto { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public DateTime? FechaActaInicioFase2 { get; set; }
        public decimal? ValorTotal { get; set; }
    }
}
