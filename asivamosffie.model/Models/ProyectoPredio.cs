using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoPredio
    {
        public int ProyectoPredioId { get; set; }
        public int? ProyectoId { get; set; }
        public int? PredioId { get; set; }
        public string EstadoJuridicoCodigo { get; set; }
        public int? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Predio Predio { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
