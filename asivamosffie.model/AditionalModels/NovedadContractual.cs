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

    public partial class NovedadContractual
    {
        [NotMapped]
        public string TipoNovedadNombre { get; set; }

        [NotMapped]
        public string EstadoNovedadNombre { get; set; }

        [NotMapped]
        public List<VProyectosXcontrato> ProyectosContrato { get; set; }

        [NotMapped]
        public VProyectosXcontrato ProyectosSeleccionado { get; set; }

        [NotMapped]
        public string NovedadesSeleccionadas { get; set; }

        [NotMapped]
        public string InstanciaNombre { get; set; }

        [NotMapped]
        public NovedadContractualObservaciones ObservacionApoyo { get; set; }

        [NotMapped]
        public NovedadContractualObservaciones ObservacionSupervisor { get; set; }
        
        [NotMapped]
        public NovedadContractualObservaciones ObservacionTramite { get; set; }

        [NotMapped]
        public NovedadContractualObservaciones ObservacionDevolucion { get; set; }

        [NotMapped]
        public NovedadContractualObservaciones ObservacionDevolucionTramite { get; set; }

        [NotMapped]
        public bool? RegistroCompletoRevisionJuridica { get; set; }

        [NotMapped]
        public bool? RegistroCompletoInformacionBasica { get; set; }

        [NotMapped]
        public bool? RegistroCompletoSoporte { get; set; }

        [NotMapped]
        public bool? RegistroCompletoDescripcion { get; set; }

        [NotMapped]
        public bool? RegistroCompletoDevolucionTramite { get; set; }

        [NotMapped]
        public bool? RegistroCompletoFirmas { get; set; }

        [NotMapped]
        public bool? RegistroCompletoDetallar { get; set; }

        [NotMapped]
        public List<SesionComiteSolicitud> sesionComiteSolicitud { get; set; }

        [NotMapped]
        public string EstadoProcesoNombre { get; set; }

        [NotMapped]
        public string NombreAbogado { get; set; }

        [NotMapped]
        public bool? VaComite { get; set; }

    }

}
