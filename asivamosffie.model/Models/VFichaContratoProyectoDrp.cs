using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaContratoProyectoDrp
    {
        public int? ContratoId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public int? AportanteId { get; set; }
        public bool? EsDrpOriginal { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int ContratacionId { get; set; }
        public string LlaveMen { get; set; }
        public string TipoIntervencion { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string Sede { get; set; }
        public string InstitucionEducativa { get; set; }
        public int? ProyectoId { get; set; }
        public int? DisponibilidadPresupuestalId { get; set; }
        public string NumeroDrp { get; set; }
        public string Nombre { get; set; }
        public string TipoUsoCodigo { get; set; }
        public decimal? ValorUso { get; set; }
        public int? ComponenteUsoId { get; set; }
        public decimal? Saldo { get; set; }
    }
}
