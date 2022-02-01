using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoPreparacionPreconstruccion
    {
        public int ProyectoId { get; set; }
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public string NombrePerfil { get; set; }
        public string CodigoPerfil { get; set; }
        public int? CantidadHvRequeridas { get; set; }
        public int? CantidadHvRecibidas { get; set; }
        public int? CantidadHvAprobadas { get; set; }
        public string RutaSoporte { get; set; }
    }
}
