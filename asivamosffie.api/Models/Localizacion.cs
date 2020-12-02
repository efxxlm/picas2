using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class Localizacion
    {
        public Localizacion()
        {
            CofinanciacionAportanteDepartamento = new HashSet<CofinanciacionAportante>();
            CofinanciacionAportanteMunicipio = new HashSet<CofinanciacionAportante>();
            Proyecto = new HashSet<Proyecto>();
        }

        public string LocalizacionId { get; set; }
        public string Descripcion { get; set; }
        public string IdPadre { get; set; }
        public decimal? Nivel { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<CofinanciacionAportante> CofinanciacionAportanteDepartamento { get; set; }
        public virtual ICollection<CofinanciacionAportante> CofinanciacionAportanteMunicipio { get; set; }
        public virtual ICollection<Proyecto> Proyecto { get; set; }
    }
}
