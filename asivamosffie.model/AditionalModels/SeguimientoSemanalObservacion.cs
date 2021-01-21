using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalObservacion
    {
        [NotMapped]
        public bool TieneObservacion { get; set; }

        //[NotMapped]
        //public string NumeroSolicitudFormat { get; set; }
    }
}
