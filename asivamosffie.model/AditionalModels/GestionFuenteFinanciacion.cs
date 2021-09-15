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

        [NotMapped]
        public dynamic SaldoFuente { get; set; }   
        
        [NotMapped]
        public dynamic ConsignadoEnFuente { get; set; }

        [NotMapped]
        public dynamic TipoAportante { get; set; }

        [NotMapped]
        public int CofinanciacionAportanteId { get; set; }

    }
}

