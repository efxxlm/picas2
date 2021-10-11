using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class VUsoHistorico
    {
        [NotMapped]
        public int ComponenteUsoId { get; set; }

        [NotMapped]
        public int ComponenteUsoNovedadId { get; set; }

        [NotMapped]
        public decimal ValorLiberar { get; set; }

        [NotMapped]
        public bool EsNovedad { get; set; }

        [NotMapped]
        public int ComponenteUsoHistoricoId { get; set; }

        [NotMapped]
        public int ComponenteUsoNovedadHistoricoId { get; set; }

    }
}
