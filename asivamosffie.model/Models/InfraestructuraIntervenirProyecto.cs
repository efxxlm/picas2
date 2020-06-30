using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InfraestructuraIntervenirProyecto
    {
        public int InfraestrucutraIntervenirProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public string InfraestructuraCodigo { get; set; }
        public int Cantidad { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public int PlazoMesesObra { get; set; }
        public int PlazoDiasObra { get; set; }
        public int PlazoMesesInterventoria { get; set; }
        public int PlazoDiasInterventoria { get; set; }
        public string CoordinacionResponsableCodigo { get; set; }

        public virtual Proyecto Proyecto { get; set; }
    }
}
