using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class VUsosHistorico
    {
        [NotMapped]
        public List<VUsoHistorico> UsosHistorico { get; set; }

    }
}
