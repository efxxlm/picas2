using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalListaChequeo
    {
        public int InformeFinalListaChequeoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Nombre { get; set; }
        public int? Posicion { get; set; }
        public string MensajeAyuda { get; set; }
    }
}
