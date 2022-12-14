using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalInterventoria
    {
        [NotMapped]
        public string CalificacionCodigoString { get; set; }

        [NotMapped]
        public string ValidacionCodigoString { get; set; }

        [NotMapped]
        public string AprobacionCodigoString { get; set; }

        [NotMapped]
        public string EstadoValidacion { get; set; }

        [NotMapped]
        public bool RegistroCompletoValidacion { get; set; }

        [NotMapped]
        public int InformeFinalInterventoriaObservacionesId { get; set; }

        [NotMapped]
        public bool TieneObservacionNoCumple { get; set; }

        [NotMapped]
        public bool Semaforo { get; set; }

        [NotMapped]
        public virtual InformeFinalInterventoriaObservaciones ObservacionVigenteSupervisor { get; set; }

        [NotMapped]
        public virtual ICollection<InformeFinalInterventoriaObservaciones> HistorialInformeFinalInterventoriaObservaciones { get; set; }

        [NotMapped]
        public bool Archivado { get; set; }
    }
}
