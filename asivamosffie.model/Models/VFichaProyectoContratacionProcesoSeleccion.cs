using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoContratacionProcesoSeleccion
    {
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string NumeroProceso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string TipoProceso { get; set; }
        public string Objeto { get; set; }
        public string AlcanceParticular { get; set; }
        public bool? EsDistribucionGrupos { get; set; }
        public int CantidadGrupos { get; set; }
        public string UrlSoporte { get; set; }
    }
}
