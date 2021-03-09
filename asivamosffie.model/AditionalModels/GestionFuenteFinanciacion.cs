using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class GestionFuenteFinanciacion
    {
        [NotMapped]
        public dynamic FuenteNombre { get; set; }

        [NotMapped]
        public dynamic AportanteNombre { get; set; }


 
    }
}

