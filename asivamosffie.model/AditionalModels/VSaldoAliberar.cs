using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class VSaldoAliberar
    {
        [NotMapped]
        public string? NombreAportante { get; set; }

        [NotMapped]
        public string? NombreFuente { get; set; }

        [NotMapped]
        public decimal? SaldoPresupuestal { get; set; }

    }
}
