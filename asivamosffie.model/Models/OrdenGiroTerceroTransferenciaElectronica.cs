using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroTerceroTransferenciaElectronica
    {
        public int OrdenGiroTerceroTransferenciaElectronicaId { get; set; }
        public string TitularCuenta { get; set; }
        public string TitularNumeroIdentificacion { get; set; }
        public string NumeroCuenta { get; set; }
        public string BancoCodigo { get; set; }
        public bool? EsCuentaAhorros { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroTerceroId { get; set; }

        public virtual OrdenGiroTercero OrdenGiroTercero { get; set; }
    }
}
