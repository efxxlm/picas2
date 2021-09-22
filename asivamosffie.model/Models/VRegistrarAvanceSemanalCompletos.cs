using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarAvanceSemanalCompletos
    {
        public int ContratacionProyectoId { get; set; }
        public int? CantidadSemanas { get; set; }
        public int? SeguimientoSemanalId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CantidadSemanasCompletas { get; set; }
        public int Completado { get; set; }
    }
}
