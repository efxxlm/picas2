using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalRegistrarComiteObra
    {
        public int SeguimientoSemanalRegistrarComiteObraId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public DateTime? FechaComite { get; set; }
        public string NumeroComite { get; set; }
        public string UrlSoporteComite { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
