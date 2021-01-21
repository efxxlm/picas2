using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoSoporteSolicitud
    {
        public int SolicitudPagoSoporteSolicitudId { get; set; }
        public int? SolicitudPagoId { get; set; }
        public string UrlSoporte { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPago SolicitudPago { get; set; }
    }
}
