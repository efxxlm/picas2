using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ComponenteFuenteNovedad
    {
        [NotMapped] 
        public FuenteFinanciacion FuenteFinanciacion { get; set; }

        [NotMapped]
        public decimal? ValorFuente { get; set; }
    }

}
