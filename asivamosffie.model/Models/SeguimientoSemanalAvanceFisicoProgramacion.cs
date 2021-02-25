using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalAvanceFisicoProgramacion
    {
        public int SeguimientoSemanalAvanceFisicoProgramacionId { get; set; }
        public int SeguimientoSemanalAvanceFisicoId { get; set; }
        public int ProgramacionId { get; set; }
        public decimal? AvanceFisicoCapitulo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Programacion Programacion { get; set; }
        public virtual SeguimientoSemanalAvanceFisico SeguimientoSemanalAvanceFisico { get; set; }
    }
}
