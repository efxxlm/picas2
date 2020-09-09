using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Solicitud
    {
        public int SolicitudId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoCodigo { get; set; }
        public bool EsCompleto { get; set; }
        public DateTime FechaTramite { get; set; }
        public DateTime? FechaEnvioDocumentacion { get; set; }
        public string Observaciones { get; set; }
        public string RutaMinuta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
