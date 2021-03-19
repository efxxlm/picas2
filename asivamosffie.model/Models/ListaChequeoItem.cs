using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ListaChequeoItem
    {
        public ListaChequeoItem()
        {
            ListaChequeoListaChequeoItem = new HashSet<ListaChequeoListaChequeoItem>();
            PlanesProgramasListaChequeoRespuesta = new HashSet<PlanesProgramasListaChequeoRespuesta>();
            SolicitudPagoListaChequeoRespuesta = new HashSet<SolicitudPagoListaChequeoRespuesta>();
        }

        public int ListaChequeoItemId { get; set; }
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<ListaChequeoListaChequeoItem> ListaChequeoListaChequeoItem { get; set; }
        public virtual ICollection<PlanesProgramasListaChequeoRespuesta> PlanesProgramasListaChequeoRespuesta { get; set; }
        public virtual ICollection<SolicitudPagoListaChequeoRespuesta> SolicitudPagoListaChequeoRespuesta { get; set; }
    }
}
