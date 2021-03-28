using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroTercero
    {
        public OrdenGiroTercero()
        {
            OrdenGiro = new HashSet<OrdenGiro>();
            OrdenGiroTerceroChequeGerencia = new HashSet<OrdenGiroTerceroChequeGerencia>();
            OrdenGiroTerceroTransferenciaElectronica = new HashSet<OrdenGiroTerceroTransferenciaElectronica>();
        }

        public int OrdenGiroTerceroId { get; set; }
        public string MedioPagoGiroCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroId { get; set; }

        public virtual OrdenGiro OrdenGiroNavigation { get; set; }
        public virtual ICollection<OrdenGiro> OrdenGiro { get; set; }
        public virtual ICollection<OrdenGiroTerceroChequeGerencia> OrdenGiroTerceroChequeGerencia { get; set; }
        public virtual ICollection<OrdenGiroTerceroTransferenciaElectronica> OrdenGiroTerceroTransferenciaElectronica { get; set; }
    }
}
