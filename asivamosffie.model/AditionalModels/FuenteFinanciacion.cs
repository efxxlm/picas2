using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class FuenteFinanciacion
    {
        [NotMapped]
        public string FuenteRecursosString { get; set; }

        [NotMapped]
        public decimal saldoFuente { get; set; }

        [NotMapped]
        public string NombreFuente { get; set; }

        [NotMapped]
        public bool AsociadoASolicitud { get; set; }


        [NotMapped]
        public decimal ComprometidoEnDdp { get; set; }

        [NotMapped]
        public decimal SaldoActual { get; set; }
    }

}
