﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyectoAportante
    {
        [NotMapped]
        public decimal SaldoDisponible { get; set; }

        [NotMapped]
        public string NombreAportante { get; set; }

    }
}
