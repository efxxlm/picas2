using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace asivamosffie.model.Models
{
    public partial class DefensaJudicialSeguimiento
    {
        [NotMapped]
        public string NumeroProceso { get; set; }

        [NotMapped]
        public string TipoAccionCodigoNombre { get; set; }

        [NotMapped]
        public string JurisdiccionCodigoNombre { get; set; }

    }
}
