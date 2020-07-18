using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Documento
    {
        public int DocumentoId { get; set; }
        public int OrigenId { get; set; }
        public int PadreId { get; set; }
        public string Nombre { get; set; }
        public int Tamano { get; set; }
        public string Tipo { get; set; }
        public string CodigoConsulta { get; set; }
    }
}
