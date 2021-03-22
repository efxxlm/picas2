using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorUsosFasesAportanteProyecto
    {
        public int ContratoId { get; set; }
        public decimal? ValorUso { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroSolicitud { get; set; }
        public string Descripcion { get; set; }
        public bool? EsPreConstruccion { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
    }
}
