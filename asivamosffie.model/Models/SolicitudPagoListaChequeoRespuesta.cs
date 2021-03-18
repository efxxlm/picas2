using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoListaChequeoRespuesta
    {
        public int SolicitudPagoListaChequeoRespuestaId { get; set; }
        public int SolicitudPagoListaChequeoId { get; set; }
        public int ListaChequeoItemId { get; set; }
        public string RespuestaCodigo { get; set; }
        public string Observacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ListaChequeoItem ListaChequeoItem { get; set; }
        public virtual SolicitudPagoListaChequeo SolicitudPagoListaChequeo { get; set; }
    }
}
