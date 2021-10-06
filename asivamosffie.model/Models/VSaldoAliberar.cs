using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldoAliberar
    {
        public string NumeroDrp { get; set; }
        public int ComponenteUsoId { get; set; }
        public string CodigoUso { get; set; }
        public decimal ValorUso { get; set; }
        public string NombreUso { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public decimal? Saldo { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
    }
}
