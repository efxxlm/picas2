using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class TipoDominio
    {
        public TipoDominio()
        {
            Dominio = new HashSet<Dominio>();
        }

        public int TipoDominioId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<Dominio> Dominio { get; set; }
    }
}
