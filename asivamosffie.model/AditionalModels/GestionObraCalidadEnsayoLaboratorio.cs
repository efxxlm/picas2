using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class GestionObraCalidadEnsayoLaboratorio
    {
        [NotMapped]
        public string LlaveMen { get; set; }

        [NotMapped]
        public int NumeroLaboratorio { get; set; }
    }
}

