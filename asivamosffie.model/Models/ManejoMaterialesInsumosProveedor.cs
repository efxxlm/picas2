using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ManejoMaterialesInsumosProveedor
    {
        public int ManejoMaterialesInsumosProveedorId { get; set; }
        public int? ManejoMaterialesInsumosId { get; set; }
        public string Proveedor { get; set; }
        public bool? RequierePermisosAmbientalesMineros { get; set; }
        public string UrlRegistroFotografico { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ManejoMaterialesInsumos ManejoMaterialesInsumos { get; set; }
    }
}
