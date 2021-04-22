using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorConstruccionXproyectoContrato
    {
        public int ProyectoId { get; set; }
        public decimal? ValorConstruccion { get; set; }
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public string NumeroSolicitud { get; set; }
        public string NumeroContrato { get; set; }
    }
}
