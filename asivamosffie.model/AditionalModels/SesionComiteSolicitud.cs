﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models; 
using System.ComponentModel.DataAnnotations.Schema;
namespace asivamosffie.model.Models
{
    public partial class SesionComiteSolicitud
    {

        [NotMapped]
        public string NumeroSolicitud { get; set; }
        [NotMapped]
        public DateTime? FechaSolicitud { get; set; }
    }
    
}
