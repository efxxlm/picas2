using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ListaChequeoItem
    {
        public ListaChequeoItem()
        {
            ListaChequeoListaChequeoItem = new HashSet<ListaChequeoListaChequeoItem>();
        }

        public int ListaChequeoItemId { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<ListaChequeoListaChequeoItem> ListaChequeoListaChequeoItem { get; set; }
    }
}
