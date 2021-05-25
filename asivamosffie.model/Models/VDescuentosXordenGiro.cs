using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDescuentosXordenGiro
    {
        public int OrdenGiroId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public string Nombre { get; set; }
        public decimal ValorDescuento { get; set; }
    }
}
