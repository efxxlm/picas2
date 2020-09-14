using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class ListAportantes
    {
        public int? ContratacionProyectoId { get; set; }
        public int ContratacionProyectoAportanteId { get; set; }
        public int? ContratacionId { get; set; }
        public int CofinanciacionAportanteId { get; set; }
        public int? TipoAportanteId { get; set; }
        public string TipoAportanteText { get; set; }
        public int? NombreAportanteId { get; set; }
        public string NombreAportante { get; set; }
        public decimal ValorAporte { get; set; }

    }
}
