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
        public string NombreRepresentanteLegal { get; set; }
        public string CedulaRepresentanteLegal { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
        public string NombreRepresentanteLegal { get; set; }
        public string CedulaRepresentanteLegal { get; set; }
    }
}
