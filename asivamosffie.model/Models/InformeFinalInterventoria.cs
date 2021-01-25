using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalInterventoria
    {
        public int InformeFinalInterventoriaId { get; set; }
        public int InformeFinalId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CalificacionCodigo { get; set; }
        public int? InformeFinalAnexoId { get; set; }
        public string ObservacionNoCumple { get; set; }
        public string ObservacionSupervisor { get; set; }
        public int InformeFinalListaChequeoId { get; set; }

        public virtual InformeFinal InformeFinal { get; set; }
        public virtual InformeFinalAnexo InformeFinalAnexo { get; set; }
        public virtual InformeFinalListaChequeo InformeFinalListaChequeo { get; set; }
    }
}
