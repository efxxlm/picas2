using System;
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
        public ConstruccionObservacion ObservacionDiagnosticoApoyo { get; set; }
        
        [NotMapped]
        public ConstruccionObservacion ObservacionDiagnosticoSupervisor { get; set; }
        
        [NotMapped]
        public ConstruccionObservacion ObservacionPlanesProgramasApoyo { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionPlanesProgramasSupervisor { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionManejoAnticipoApoyo { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionManejoAnticipoSupervisor { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionProgramacionObraApoyo { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionProgramacionObraSupervisor { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionFlujoInversionApoyo { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionFlujoInversionSupervisor { get; set; }

        [NotMapped]
        public ConstruccionObservacion ObservacionDevolucionDiagnostico { get; set; }
        
        [NotMapped]
        public ConstruccionObservacion ObservacionDevolucionPlanesProgramas { get; set; }
        
    }
}
