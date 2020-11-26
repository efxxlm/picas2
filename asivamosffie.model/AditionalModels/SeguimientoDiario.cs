using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoDiario
    {
        [NotMapped]
        public string DisponibilidadMaterial { get; set; }
        
        [NotMapped]
        public string DisponibilidadEquipo { get; set; }

        [NotMapped]
        public string CausaBajaDisponibilidadMaterial { get; set; }
        
        [NotMapped]
        public string CausaBajaDisponibilidadEquipo { get; set; }

        [NotMapped]
        public string CausaBajaDisponibilidadProductividad { get; set; }

        [NotMapped]
        public string ProductividadNombre { get; set; }

        [NotMapped]
        public string EstadoNombre { get; set; } 
    }

}
