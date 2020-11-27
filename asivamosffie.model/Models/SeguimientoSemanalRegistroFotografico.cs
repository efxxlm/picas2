using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalRegistroFotografico
    {
        public int SeguimientoSemanalRegistroFotografico1 { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public string UrlSoporteFotografico { get; set; }
        public string Descripcion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
