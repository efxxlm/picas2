﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SesionSolicitudCompromiso
    {
        [NotMapped]
        public string GestionRealizada { get; set; }

        [NotMapped]
        public string TipoCompromiso { get; set; }
    }

}
