using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanal
    {
         
        [NotMapped]
        public virtual List<dynamic> AvanceAcumulado { get; set; }     

        [NotMapped]
        public string ComiteObraGenerado { get; set; }

        [NotMapped]
        public int CantidadTotalDiasActividades { get; set; }
    }
}
