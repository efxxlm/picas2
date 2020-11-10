﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using asivamosffie.model.Models; 
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class ContratoConstruccion
    {
        [NotMapped]
        public bool? EsCompletoDiagnostico  { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionDiagnostico { get; set; }
        
        [NotMapped]
        public ConstruccionObservacion ObservacionPlanesProgramas { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionManejoAnticipo { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionProgramacionObra { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionFlujoInversion { get; set; }
        
    }
}
