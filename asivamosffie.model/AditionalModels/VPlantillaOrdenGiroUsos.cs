using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class VPlantillaOrdenGiroUsos
    {
        [NotMapped]
        public decimal? ValorNetoGiroConDescuentos
        {
            get => ValorConcepto - DescuentoAns - DescuentoOtros - DescuentoReteFuente;
        } 
    } 
}
