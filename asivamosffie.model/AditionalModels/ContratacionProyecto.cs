using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyecto
    { 
        [NotMapped]
        public IFormFile pFile { get; set; }

        [NotMapped]
        public ProyectoGrilla ProyectoGrilla { get; set; }

        [NotMapped]
        public string PorcentajeAvanceObraString { get; set; }

        [NotMapped]
        public string TieneAlgunRegistro { get; set; }
        [NotMapped]
        public Boolean faseConstruccionNotMapped { get; set; }
        [NotMapped]
        public Boolean fasePreConstruccionNotMapped { get; set; }
        
    }
}
