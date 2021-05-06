using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TablaUsoFuenteAportante
    {
        public List<Usos> Usos { get; set; }
    }

    public partial class Usos
    {
        public string NombreUso { get; set; }
        public string TipoUsoCodigo { get; set; }
        public string FuenteFinanciacion { get; set; }
        public int? FuenteFinanciacionId { get; set; }
         
        public List<Fuentes> Fuentes { get; set; }
    }
    public partial class Fuentes
    {
        public string NombreUso { get; set; }
        public string TipoUsoCodigo { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string NombreFuente { get; set; }
        public List<Aportante> Aportante { get; set; }
    }
    public partial class Aportante
    {
        public int? FuenteFinanciacionId { get; set; }
        public int? AportanteId { get; set; }
        public string NombreAportante { get; set; }
        public List<ValorUso> ValorUso { get; set; }
        public List<SaldoActualUso> SaldoActualUso { get; set; }
    }

    public partial class ValorUso
    {
        public int? AportanteId { get; set; }
        public string Valor { get; set; }
    }

    public partial class SaldoActualUso
    {
        public int? AportanteId { get; set; }
        public string Valor { get; set; }
    }
}
