using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoCargarFormaPago
    {
        [NotMapped]
        public bool TieneFase1 { get; set; } 
    }

}
