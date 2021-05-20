using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProyectosBalance
    {
        public DateTime? FechaTerminacionProyecto { get; set; }
        public string LlaveMen { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string SedeEducativa { get; set; }
        public int ProyectoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoBalance { get; set; }
        public string EstadoBalanceCodigo { get; set; }
        public int? NumeroTraslado { get; set; }
        public int? BalanceFinancieroId { get; set; }
    }
}
