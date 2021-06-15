using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroPago
    {
        public int OrdenGiroPagoId { get; set; }
        public int OrdenGiroId { get; set; }
        public int RegistroPagoId { get; set; }

        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual CarguePago RegistroPago { get; set; }
    }
}
