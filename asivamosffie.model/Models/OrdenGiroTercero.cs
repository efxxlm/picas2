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
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroTerceroTransferenciaElectronicaId { get; set; }
        public int? OrdenGiroTerceroChequeGerenciaId { get; set; }

        public virtual OrdenGiroTerceroChequeGerencia OrdenGiroTerceroChequeGerencia { get; set; }
        public virtual OrdenGiroTerceroTransferenciaElectronica OrdenGiroTerceroTransferenciaElectronica { get; set; }
        public virtual ICollection<OrdenGiro> OrdenGiro { get; set; }
    }
}
