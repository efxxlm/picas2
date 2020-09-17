using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionProponente
    {
        public int ProcesoSeleccionProponenteId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string TipoProponenteCodigo { get; set; }
        public string NombreProponente { get; set; }
        public string TipoIdentificacionCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public string DireccionProponente { get; set; }
        public string TelefonoProponente { get; set; }
        public string EmailProponente { get; set; }
        public string CedulaRepresentanteLegal { get; set; }
        public string NombreRepresentanteLegal { get; set; }
<<<<<<< HEAD
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
