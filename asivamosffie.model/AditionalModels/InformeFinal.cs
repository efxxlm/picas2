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
    public partial class InformeFinal
    {
        [NotMapped]
        public string EstadoValidacionString { get; set; }

        [NotMapped]
        public string EstadoInformeString { get; set; }

        [NotMapped]
        public string EstadoAprobacionString { get; set; }

        [NotMapped]
        public string EstadoCumplimientoString { get; set; }

        [NotMapped]
        public string EstadoEntregaETCString { get; set; }

        [NotMapped]
        public virtual ICollection<InformeFinalObservaciones> InformeFinalObservacionesSupervisor { get; set; }

    }
}
