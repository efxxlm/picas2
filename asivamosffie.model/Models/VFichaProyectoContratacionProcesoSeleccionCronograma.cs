using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoContratacionProcesoSeleccionCronograma
    {
        public int ProcesoSeleccionId { get; set; }
        public int ProcesoSeleccionCronogramaId { get; set; }
        public string EstapaCodigo { get; set; }
        public string NombreEtapa { get; set; }
        public DateTime? FechaEtapa { get; set; }
    }
}
