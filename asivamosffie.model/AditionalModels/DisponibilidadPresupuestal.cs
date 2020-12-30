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
        /*jflorez, dejo el notmapped en el nombre par no generar confusi�n*/
        [NotMapped]
        public DateTime FechaComiteTecnicoNotMapped { get; set; }
        [NotMapped]
        public string stringAportante{ get; set; }

        [NotMapped]
        public string[] observacionesRechazo{ get; set; }
    }
}
