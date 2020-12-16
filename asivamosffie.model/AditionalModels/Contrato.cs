﻿using System;
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
    public partial class Contrato
    {
        [NotMapped]
        public IFormFile pFile { get; set; }
       
        [NotMapped]
        public DateTime? FechaEnvioFirmaFormat { get; set; }

        [NotMapped]
        public DateTime? FechaFirmaContratistaFormat { get; set; }

        [NotMapped]
        public DateTime? FechaFirmaFiduciariaFormat { get; set; }


        [NotMapped]
        public DateTime? FechaFirmaContratoFormat { get; set; }

        [NotMapped]
        public List<CofinanicacionAportanteGrilla> ListAportantes{ get; set; }
    }
}
