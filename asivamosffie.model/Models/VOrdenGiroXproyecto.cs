using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VOrdenGiroXproyecto
    {
        public int OrdenGiroId { get; set; }
        public string NumeroOrdenGiro { get; set; }
        public int ProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string Nombre { get; set; }
    }
}
