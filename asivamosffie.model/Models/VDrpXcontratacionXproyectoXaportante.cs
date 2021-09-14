using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDrpXcontratacionXproyectoXaportante
    {
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public string LlaveMen { get; set; }
        public int ProyectoId { get; set; }
        public int? AportanteId { get; set; }
        public decimal? ValorAporte { get; set; }
    }
}
