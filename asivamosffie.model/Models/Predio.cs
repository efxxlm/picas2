using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Predio
    {
        public Predio()
        {
            Proyecto = new HashSet<Proyecto>();
            ProyectoPredio = new HashSet<ProyectoPredio>();
        }

        public int PredioId { get; set; }
        public int? InstitucionEducativaSedeId { get; set; }
        public string TipoPredioCodigo { get; set; }
        public string UbicacionLatitud { get; set; }
        public string UbicacionLongitud { get; set; }
        public string Direccion { get; set; }
        public string DocumentoAcreditacionCodigo { get; set; }
        public string NumeroDocumento { get; set; }
        public string CedulaCatastral { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual InstitucionEducativaSede InstitucionEducativaSede { get; set; }
        public virtual ICollection<Proyecto> Proyecto { get; set; }
        public virtual ICollection<ProyectoPredio> ProyectoPredio { get; set; }
    }
}
