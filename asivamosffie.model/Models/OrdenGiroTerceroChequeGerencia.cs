using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroTerceroChequeGerencia
    {
        public OrdenGiroTerceroChequeGerencia()
        {
            OrdenGiroTercero = new HashSet<OrdenGiroTercero>();
        }

        public int OrdenGiroTerceroChequeGerenciaId { get; set; }
        public string NombreBeneficiario { get; set; }
        public string NumeroIdentificacionBeneficiario { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual ICollection<OrdenGiroTercero> OrdenGiroTercero { get; set; }
    }
}
