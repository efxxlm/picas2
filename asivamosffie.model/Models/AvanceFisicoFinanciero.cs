using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AvanceFisicoFinanciero
    {
        public int AvanceFisicoFinancieroId { get; set; }
        public DateTime? FechaReporte { get; set; }
        public string VariableCodigo { get; set; }
        public string Causa { get; set; }
        public int? HorasRetraso { get; set; }
        public string Observaciones { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
