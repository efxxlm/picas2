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

    }

}
