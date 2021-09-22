using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDrpXcontratacionXproyectoXfaseXconceptoXusos
    {
        public DateTime? FechaCreacion { get; set; }
        public int ContratacionId { get; set; }
        public string LlaveMen { get; set; }
        public int ProyectoId { get; set; }
        public int? DisponibilidadPresupuestalId { get; set; }
        public string NumeroDrp { get; set; }
        public string Nombre { get; set; }
        public string ConceptoPagoCodigo { get; set; }
        public string TipoUsoCodigo { get; set; }
        public int ComponenteUsoId { get; set; }
        public decimal ValorUso { get; set; }
        public bool? EsPreConstruccion { get; set; }
        public decimal? Saldo { get; set; }
    }
}
