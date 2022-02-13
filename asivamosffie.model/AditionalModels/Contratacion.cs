using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class Contratacion
    {

        [NotMapped]
        public bool EsExpensa { get; set; }

        [NotMapped]
        public IFormFile? pFile { get; set; }

        [NotMapped]
        public List<SesionComiteSolicitud> sesionComiteSolicitud { get; set; }
        /*jflorez, dejo el notmapped en el nombre par no generar confusión*/
        [NotMapped]
        public DateTime? FechaComiteTecnicoNotMapped { get; set; }
         
        [NotMapped]
        public string ObservacionNotMapped { get; set; }

        [NotMapped]
        public string EstadoSolicitudNombre { get; set; }
    }
}
