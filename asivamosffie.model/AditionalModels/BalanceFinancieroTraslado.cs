using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class BalanceFinancieroTraslado
    {
        [NotMapped]
        public string NumeroContrato { get; set; }

        [NotMapped]
        public string NumeroOrdenGiro { get; set; }

        [NotMapped]
        public List<TablaDRP> TablaDRP { get; set; }

        [NotMapped]
        public int SolicitudPagoId { get; set; }
    }
}
