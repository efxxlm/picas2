using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class PlazoContratacion
    {
        public PlazoContratacion()
        {
            Contratacion = new HashSet<Contratacion>();
        }

        public int PlazoContratacionId { get; set; }
        public int PlazoDias { get; set; }
        public int PlazoMeses { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<Contratacion> Contratacion { get; set; }
    }
}
