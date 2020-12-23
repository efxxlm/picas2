using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObraSocial
    {
        public int SeguimientoSemanalGestionObraSocialId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public bool? SeRealizaronReuniones { get; set; }
        public string TemaComunidad { get; set; }
        public string Conclusion { get; set; }
        public int? CantidadEmpleosDirectos { get; set; }
        public int? CantidadEmpleosIndirectos { get; set; }
        public int? CantidadTotalEmpleos { get; set; }
        public string UrlSoporteGestion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SeguimientoSemanalGestionObra SeguimientoSemanalGestionObra { get; set; }
    }
}
