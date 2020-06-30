using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CofinanciacionAportante
    {
        public CofinanciacionAportante()
        {
            CofinanciacionDocumento = new HashSet<CofinanciacionDocumento>();
        }

        public int CofinanciacionAportanteId { get; set; }
        public int CofinanciacionId { get; set; }
        public int TipoAportanteId { get; set; }
        public int? NombreAportanteId { get; set; }
        public int MunicipioId { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Cofinanciacion Cofinanciacion { get; set; }
        public virtual ICollection<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
    }
}
