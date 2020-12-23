using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
        [NotMapped]
        public string TipoProcesoCodigoNombre { get; set; }

        [NotMapped]
        public string ContratosAsociados { get; set; }

        [NotMapped]
        public string FuenteProceso { get; set; }
        
        [NotMapped]
        public string TipoAccionCodigoNombre { get; set; }

        [NotMapped]
        public string JurisdiccionCodigoNombre { get; set; }

        [NotMapped]
        public string FechaCreacionFormat { get; set; }
        
    }
}
