using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VPermisosMenus
    {
        public int MenuPerfilId { get; set; }
        public int PerfilId { get; set; }
        public string RutaFormulario { get; set; }
        public bool? TienePermisoCrear { get; set; }
        public bool? TienePermisoLeer { get; set; }
        public bool? TienePermisoEditar { get; set; }
        public bool? TienePermisoEliminar { get; set; }
    }
}
