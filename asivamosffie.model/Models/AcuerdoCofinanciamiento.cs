using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AcuerdoCofinanciamiento
    {
        public AcuerdoCofinanciamiento()
        {
            Aportante = new HashSet<Aportante>();
        }

        public int AcuerdoCofinanciacionId { get; set; }
        public string VigenciaCodigo { get; set; }
        public int CantidadAportantes { get; set; }
        public decimal ValorTotal { get; set; }
        public string EstadoCodigo { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<Aportante> Aportante { get; set; }
    }
}
