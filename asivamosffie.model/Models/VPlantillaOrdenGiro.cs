using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VPlantillaOrdenGiro
    {
        public bool? EstaAprobada { get; set; }
        public int? ContratacionId { get; set; }
        public int? ContratoId { get; set; }
        public string NumeroDdp { get; set; }
        public string InstitucionEducativa { get; set; }
        public int? ProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public int? OrdenGiroId { get; set; }
        public string ConsecutivoFfie { get; set; }
        public string NombreCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public string CodigoSifi { get; set; }
        public string TipoCuenta { get; set; }
        public string NombreBanco { get; set; }
        public string TerceroCausasionIdentificacion { get; set; }
        public string TerceroCausasionNombre { get; set; }
        public string Numerofactura { get; set; }
        public string Concepto { get; set; }
        public int? AportanteId { get; set; }
        public string FormaPago { get; set; }
        public decimal? ValorConcepto { get; set; }
        public decimal? ValorNetoGiro { get; set; }
        public decimal? DescuentoReteFuente { get; set; }
        public decimal? DescuentoAns { get; set; }
        public decimal? DescuentoOtros { get; set; }
        public string DescuentoOtrosNombre { get; set; }
        public bool? EsCuentaAhorros { get; set; }
        public string NombreBancoTercero { get; set; }
        public string NumeroCuentaTercero { get; set; }
        public string IdentificacionTercero { get; set; }
        public string TitularTercero { get; set; }
    }
}
