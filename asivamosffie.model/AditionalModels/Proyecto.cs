using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class Proyecto
    {
        [NotMapped]
        public DateTime? FechaComite { get; set; }

        [NotMapped]
        public string Departamento { get; set; }

        [NotMapped]
        public string Municipio { get; set; }

    }
}
