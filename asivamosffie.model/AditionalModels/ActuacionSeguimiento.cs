using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class ActuacionSeguimiento
    {
        [NotMapped]
        public string NumeroActuacionFormat { get; set; }

        [NotMapped]
        public string NumeroReclamacion { get; set; }

        [NotMapped] 
        public string NumeroContrato { get; set; }
        [NotMapped]
        public string TipoControversia { get; set; }
    }
}
