using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionCotizacion
    {
        public int ProcesoSeleccionCotizacionId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string NombreOrganizacion { get; set; }
        public decimal ValorCotizacion { get; set; }
        public string Descripcion { get; set; }
        public string UrlSoporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
