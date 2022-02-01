using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDrpGeneral
    {
        public int ContratoId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public bool? EsPreConstruccion { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public string NombreUso { get; set; }
        public string TipoUsoCodigo { get; set; }
        public decimal? ValorUso { get; set; }
    }
}
