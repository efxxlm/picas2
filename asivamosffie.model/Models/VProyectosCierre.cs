﻿using System;
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
        public int ProyectoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoInforme { get; set; }
        public string EstadoInformeCod { get; set; }
    }
}
