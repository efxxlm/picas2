using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Cofinanciacion
    {
        public Cofinanciacion()
        {
            CofinanciacionAportante = new HashSet<CofinanciacionAportante>();
        }

        public int CofinanciacionId { get; set; }
        public int VigenciaCofinanciacionId { get; set; }
        public bool Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<CofinanciacionAportante> CofinanciacionAportante { get; set; }
    }
}
