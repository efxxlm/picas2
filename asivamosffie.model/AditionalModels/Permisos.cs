using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class VPermisosMenus
    {
        [NotMapped]
        public bool TienePermisoMenu { get; set; }

  
        public VPermisosMenus()
        {
            TienePermisoMenu = false; 
        }
    }
}
