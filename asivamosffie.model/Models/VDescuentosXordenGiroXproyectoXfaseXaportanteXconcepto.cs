using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VDescuentosXordenGiroXproyectoXfaseXaportanteXconcepto
    {
        public int? ContratacionId { get; set; }
        public int EsTerceroCausacion { get; set; }
        public int? Id { get; set; }
        public int? IdDescuento { get; set; }
        public int? OrdenGiroId { get; set; }
        public string TipoDescuentoCodigo { get; set; }
        public string Nombre { get; set; }
        public int? AportanteId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? ProyectoId { get; set; }
        public bool? EsPreconstruccion { get; set; }
        public string ConceptoCodigo { get; set; }
        public string CriterioCodigo { get; set; }
        public decimal ValorDescuento { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoPagoCodigo { get; set; }
    }
}
