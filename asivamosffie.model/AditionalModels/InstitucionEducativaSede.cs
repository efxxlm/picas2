using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class InstitucionEducativaSede
    {
        [NotMapped]
        public Localizacion Municipio { get; set; }
        [NotMapped]
        public Localizacion Departamento { get; set; }
        [NotMapped]
        public Localizacion Region { get; set; }
    }
}
