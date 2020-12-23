using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace asivamosffie.model.Models
{
    public partial class ControversiaActuacion
    {
        [NotMapped]
        public string NumeroActuacionFormat { get; set; }
         
    }
}
