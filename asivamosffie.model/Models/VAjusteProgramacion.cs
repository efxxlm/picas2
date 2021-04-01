using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VAjusteProgramacion
    {
        public DateTime? FechaAprobacionPoliza { get; set; }
        public string NumeroContrato { get; set; }
        public string LlaveMen { get; set; }
        public string NovedadesSeleccionadas { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string EstadoNombre { get; set; }
    }
}
