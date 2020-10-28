using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProyectoAportante
    {
        [NotMapped]
        public string NombreAportante { get; set; }

        [NotMapped]
        public string TipoAportanteNombre { get; set; }
         
        [NotMapped]
        public decimal? SaldoDisponible { get; set; }
    } 
}
