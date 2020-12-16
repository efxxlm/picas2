using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class GrupoMunicipios
    {
        public int GrupoMunicipiosId { get; set; }
        public int ProcesoSeleccionGrupoId { get; set; }
        public int LocalizacionIdMunicipio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccionGrupo ProcesoSeleccionGrupo { get; set; }
    }
}
