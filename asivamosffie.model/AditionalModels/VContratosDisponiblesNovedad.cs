using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class VContratosDisponiblesNovedad
    {

        [NotMapped]
        public List<NovedadContractual> NovedadContractual { get; set; }
    }


}
