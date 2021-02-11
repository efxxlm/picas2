using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ListaChequeo
    {
        public ListaChequeo()
        {
            ListaChequeoListaChequeoItem = new HashSet<ListaChequeoListaChequeoItem>();
        }

        public int ListaChequeoId { get; set; }
        public string Nombre { get; set; }
        public string CriterioPagoCodigo { get; set; }
        public string EstadoCodigo { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<ListaChequeoListaChequeoItem> ListaChequeoListaChequeoItem { get; set; }
    }
}
