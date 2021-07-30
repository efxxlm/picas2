﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class Usuario
    {
        [NotMapped]
        public int PerfilId { get; set; }

        [NotMapped]
        public Perfil Perfil { get; set; }

        [NotMapped]
        public List<ContratoAsignado> ContratosAsignados { get; set; }

       
        [NotMapped]
        public string GetNombreCompleto
        {
            get => PrimerNombre + " " + SegundoNombre + " " + " " + PrimerApellido + " " + SegundoApellido; 
        }

    }


}
