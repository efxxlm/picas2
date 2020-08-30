using System;
using System.Collections.Generic;
using asivamosffie.model.Models; 
using System.ComponentModel.DataAnnotations.Schema;
namespace asivamosffie.model.Models
{
    public partial class SesionComiteSolicitud
    { 
        [NotMapped]
        public string NumeroSolicitud { get; set; }
        [NotMapped]
        public DateTime? FechaSolicitud { get; set; }
        [NotMapped]
        public string TipoSolicitud { get; set; } 
        [NotMapped]
        public ProcesoSeleccion ProcesoSeleccion { get; set; } 
        [NotMapped]
        public Contratacion Contratacion { get; set; }
    }
    
}
