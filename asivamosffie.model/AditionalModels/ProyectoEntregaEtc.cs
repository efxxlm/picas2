using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProyectoEntregaEtc
    {
        [NotMapped]
        public string FuenteFinanciacionString { get; set; }
        [NotMapped]
        public string NombreAportanteString { get; set; }

    }
}
