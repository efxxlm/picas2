using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class RendimientosIncorporados
    {
        public int RendimientosIncorporadosId { get; set; }
        public int CarguePagosRendimientosId { get; set; }
        public DateTime FechaRendimientos { get; set; }
        public string CuentaBancaria { get; set; }
        public decimal TotalRendimientosGenerados { get; set; }
        public decimal Incorporados { get; set; }
        public decimal ProvisionGravamenFinanciero { get; set; }
        public decimal TotalGastosBancarios { get; set; }
        public decimal TotalGravamenFinancieroDescontado { get; set; }
        public decimal Visitas { get; set; }
        public decimal RendimientoIncorporar { get; set; }
        public bool? Consistente { get; set; }

        public virtual CarguePagosRendimientos CarguePagosRendimientos { get; set; }
    }
}
