using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class PlanesProgramasListaChequeoRespuesta
    {
        public int PlanesProgramasListaChequeoRespuestaId { get; set; }
        public int ListaChequeoItemId { get; set; }
        public string RecibioRequisitoCodigo { get; set; }
        public DateTime? FechaRadicado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public bool? TieneObservaciones { get; set; }
        public string Observacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ListaChequeoItem ListaChequeoItem { get; set; }
    }
}
