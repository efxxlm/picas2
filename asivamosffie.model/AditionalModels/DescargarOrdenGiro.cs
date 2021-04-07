using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class DescargarOrdenGiro
    { 
        [NotMapped]
        public bool RegistrosAprobados { get; set; }

        [NotMapped]
        public DateTime? FechaInicial { get; set; }

        [NotMapped]
        public DateTime? FechaFinal { get; set; }
    }
}
