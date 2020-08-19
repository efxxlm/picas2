using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Plantilla
    {
        public int PlantillaId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Contenido { get; set; }
        public double? MargenArriba { get; set; }
        public double? MargenAbajo { get; set; }
        public double? MargenDerecha { get; set; }
        public double? MargenIzquierda { get; set; }
        public int? EncabezadoId { get; set; }
        public int? PieDePaginaId { get; set; }
        public int? TipoPlantillaId { get; set; }
    }
}
