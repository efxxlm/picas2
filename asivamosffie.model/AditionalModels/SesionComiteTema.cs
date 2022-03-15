using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteTema
    {
        [NotMapped]
        public string NombreResponsable { get; set; }

        [NotMapped]
        public bool? RegistroCompletoActa { get; set; }


        [NotMapped]
        public int? ComiteTecnicoFiduciarioIdMapped { get; set; }

    }

}
