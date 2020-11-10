using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionPerfil
    {
        [NotMapped]
        public string NombrePerfil { get; set; } 
    }
}
