using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroTercero
    {
        public OrdenGiroTercero()
        {
            OrdenGiro = new HashSet<OrdenGiro>();
        }

        public int OrdenGiroTerceroId { get; set; }
        public string MedioPagoGiroCodigo { get; set; }
        public string TitularCuenta { get; set; }
        public string TitularNumeroIdentificacion { get; set; }
        public string NumeroCuenta { get; set; }
        public string BancoCodigo { get; set; }
        public bool? EsCuentaAhorros { get; set; }

        public virtual ICollection<OrdenGiro> OrdenGiro { get; set; }
    }
}
