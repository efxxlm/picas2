using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class AjusteProgramacion
    {
        [NotMapped]
        public AjustePragramacionObservacion ObservacionObra { get; set; }

        [NotMapped]
        public AjustePragramacionObservacion ObservacionFlujo { get; set; }

    }
}
