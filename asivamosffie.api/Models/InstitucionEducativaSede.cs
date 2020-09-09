using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class InstitucionEducativaSede
    {
        public InstitucionEducativaSede()
        {
            Predio = new HashSet<Predio>();
            ProyectoInstitucionEducativa = new HashSet<Proyecto>();
            ProyectoSede = new HashSet<Proyecto>();
        }

        public int InstitucionEducativaSedeId { get; set; }
        public int? PadreId { get; set; }
        public string Nombre { get; set; }
        public int? CodigoDane { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<Predio> Predio { get; set; }
        public virtual ICollection<Proyecto> ProyectoInstitucionEducativa { get; set; }
        public virtual ICollection<Proyecto> ProyectoSede { get; set; }
    }
}
