using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestal
    {
        [NotMapped]
        public string EstadoSolicitudNombre { get; set; } 
    }
}
