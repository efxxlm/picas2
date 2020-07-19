using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionGrupo
    {
        public ProcesoSeleccionGrupo()
        {
            GrupoMunicipios = new HashSet<GrupoMunicipios>();
        }

        public int ProcesoSeleccionGrupoId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string NombreGrupo { get; set; }
        public string TipoPresupuestoCodigo { get; set; }
        public decimal? Valor { get; set; }
        public decimal? ValorMinimoCategoria { get; set; }
        public decimal? ValorMaximoCategoria { get; set; }
        public int PlazoMeses { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
        public virtual ICollection<GrupoMunicipios> GrupoMunicipios { get; set; }
    }
}
