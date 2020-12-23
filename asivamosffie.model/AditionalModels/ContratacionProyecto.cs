using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyecto
    {
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
