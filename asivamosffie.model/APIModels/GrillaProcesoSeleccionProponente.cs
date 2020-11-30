using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaProcesoSeleccionProponente
    {
        public int ProcesoSeleccionProponenteId { get; set; }
        public string TipoProponenteCodigo { get; set; }
        public string TipoProponenteText { get; set; }
        public string NombreProponente { get; set; }
        public string TipoIdentificacionCodigo { get; set; }
        public string TipoIdentificaciontext { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public string DireccionProponente { get; set; }
        public string TelefonoProponente { get; set; }
        public string EmailProponente { get; set; }


        public string UsuarioCreacion { get; set; }

    }
}
