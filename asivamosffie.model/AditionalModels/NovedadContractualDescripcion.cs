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

    public partial class NovedadContractualDescripcion
    {
        [NotMapped]
        public string NombreTipoNovedad { get; set; }

        [NotMapped]
        public List<Dominio> MotivosNovedad { get; set; }

        //[NotMapped]
        //public bool? RegistroCompleto { get; set; }

        [NotMapped]
        public TimeSpan? GetDiasFechaSuspension
        {
            get => (FechaFinSuspension - FechaInicioSuspension);
        }

    }

}
