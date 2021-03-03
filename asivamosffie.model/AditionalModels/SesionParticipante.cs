using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SesionParticipante
    {
        [NotMapped]
        public bool? esAprobado { get; set; }
     
    }

}
