using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDominio
    {
        public DateTime? FechaCreacion { get; set; }
        public string IdValor { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int DominioId { get; set; }
        public int Td { get; set; }
        public string NombreDominio { get; set; }
    }
}
