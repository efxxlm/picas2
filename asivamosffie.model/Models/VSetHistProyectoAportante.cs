using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSetHistProyectoAportante
    {
        public int ProyectoAportanteId { get; set; }
        public int ProyectoId { get; set; }
        public int AportanteId { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotalAportante { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public int? CofinanciacionDocumentoId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
