using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class VNovedadContractual
    {
        [NotMapped]
        public NovedadContractual novedadContractual { get; set; }

        [NotMapped]
        public bool vaComite { get; set; }


    }
}
