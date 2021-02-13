using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoListaChequeo
    {
        public SolicitudPagoListaChequeo()
        {
            SolicitudPagoListaChequeoRespuesta = new HashSet<SolicitudPagoListaChequeoRespuesta>();
        }

        public int SolicitudPagoListaChequeoId { get; set; }
        public int SolicitudPagoId { get; set; }
        public int ListaChequeoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ListaChequeo ListaChequeo { get; set; }
        public virtual SolicitudPago SolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoListaChequeoRespuesta> SolicitudPagoListaChequeoRespuesta { get; set; }
    }
}
