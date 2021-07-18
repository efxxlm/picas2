using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSeguimientoSemanalRegistrar
    {
        public int? SeguimientoSemanalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
