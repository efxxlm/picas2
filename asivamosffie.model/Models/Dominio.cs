using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Dominio
    {
        public int DominioId { get; set; }
        public int TipoDominioId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual TipoDominio TipoDominio { get; set; }
    }
}
