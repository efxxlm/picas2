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
        public string NumeroSolicitud { get; set; }

        [NotMapped]
        public string Departamento { get; set; }

        [NotMapped]
        public string Municipio { get; set; }

        [NotMapped]
        public Localizacion DepartamentoObj { get; set; }

        [NotMapped]
        public Localizacion MunicipioObj { get; set; }
        [NotMapped]
        public string tipoIntervencionString { get; set; }

        [NotMapped]
        public string sedeString { get; set; }

        [NotMapped]
        public string institucionEducativaString { get; set; }

        [NotMapped]
        public DateTime FechaInicioEtapaObra { get; set; }

        [NotMapped]
        public DateTime FechaFinEtapaObra { get; set; }

        [NotMapped]
        public decimal? ValorFaseConstruccion { get; set; }
    }
}
