using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaContratoBusquedaContrato
    {
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string Nombre { get; set; }
        public string TipoContratacion { get; set; }
    }
}
