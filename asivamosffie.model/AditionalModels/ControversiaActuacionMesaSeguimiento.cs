using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace asivamosffie.model.Models
{
    public partial class ControversiaActuacionMesaSeguimiento
    {
        [NotMapped]
        public string EstadoRegistroString { get; set; }

        [NotMapped]
        public string EstadoAvanceMesaString { get; set; }
    }
}
