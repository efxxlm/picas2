using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CuentaBancaria
    {
        public CuentaBancaria()
        {
            ControlRecurso = new HashSet<ControlRecurso>();
        }

        public int CuentaBancariaId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string NumeroCuentaBanco { get; set; }
        public string NombreCuentaBanco { get; set; }
        public string CodigoSifi { get; set; }
        public string TipoCuentaCodigo { get; set; }
        public string BancoCodigo { get; set; }
        public bool? Exenta { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
    }
}
