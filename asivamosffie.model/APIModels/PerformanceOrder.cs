using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace asivamosffie.model.APIModels
{
    /// <summary>
    /// TODO SAVE JSON AS UPPERCASE
    /// </summary>
    public class PerformanceOrder: EntryPerformanceOrder
    {
        public int Row { get; set; }
        /// <summary>
        /// Total de rendimientos generados
        /// #2  SUM x => “acumulado de rendimientos de recursos exentos”  
        ///   + SUM X =>  “acumulado de  rendimientos de recursos no exentos”
        /// </summary>
        [JsonProperty(PropertyName = "Total de rendimientos generados")]
        public decimal GeneratedPerformances
        {
            get { return ExemptPerformances + LiablePerformances; }
        }

        /// <summary>
        /// Provisión gravamen financiero
        /// Muestra el cálculo realizado entre: sumatoria de “Acumulado de aportes de recursos no exentos” 
        /// y “Acumulado de rendimientos no exentos”, el valor resultante se multiplica por 4/1000 
        /// y el resultado se le resta el valor del campo “Acumulado de gravamen financiero descontado no exentos”
        /// </summary>
        [JsonProperty(PropertyName = "Provisión gravamen financiero")]
        public decimal FinancialLienProvision
        {
            get {
                decimal sumLiables = LiableContributtions + LiablePerformances;
                decimal fourThousand = 4 / 100;
                decimal total = (sumLiables * fourThousand) - LiableDiscountedCharge;
                return total;
            }
        }

        /// <summary>
        /// Total gastos bancarios
        /// Muestra la sumatoria de la información de los campos “Acumulado de gastos Bancarios exentos”
        /// y “Acumulado de gastos Bancarios no exentos”
        /// </summary>
        [JsonProperty(PropertyName = "Total gastos bancarios")]
        public decimal BankCharges
        {
            get { return ExemptBankCharges + LiableBankCharges; }
        }

        /// <summary>
        /// Total gravamen financiero descontado
        /// Muestra la sumatoria de la información de los campos 
        /// “Acumulado de gravamen financiero descontado exentos” 
        /// y “Acumulado de gravamen financiero descontado no exentos”
        /// </summary>
        [JsonProperty(PropertyName = "Total gravamen financiero descontado")]
        public decimal DiscountedCharge
        {
            get { return ExemptDiscountedCharge + LiableDiscountedCharge; }
        }

        private decimal performancesToAdd;

        public decimal PerformancesToAdd
        {
            get { return performancesToAdd; }
            set { performancesToAdd = value; }
        }

        [JsonProperty(PropertyName = "Consistente")]
        public bool IsConsistent { get; set; }


        // TODO calculate rows here or in other inherited class

    }


    public class ManagedPerformancesOrder: PerformanceOrder{
        [JsonProperty(PropertyName = "Estado")]
        public  new string Status { get; set; }

        //[JsonProperty(PropertyName = "Fecha de rendimientos")]
        //public string PerformancesDate { get; set; }

        //[JsonProperty(PropertyName = "Número de Cuenta")]
        //public string AccountNumber { get; set; }

        ///// <summary>
        ///// Acumulado de aportes de recursos exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de aportes de recursos exentos")]

        //public decimal ExemptResources { get; set; }

        ///// <summary>
        ///// Acumulado de rendimientos exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de rendimientos exentos")]
        //public decimal ExemptPerformances { get; set; }

        ///// <summary>
        ///// Acumulado de gastos Bancarios exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de gastos Bancarios exentos")]
        //public decimal ExemptBankCharges { get; set; }

        ///// <summary>
        ///// Acumulado de gravamen financiero descontado exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de gravamen financiero descontado exentos")]
        //public decimal ExemptDiscountedCharge { get; set; }

        ///// <summary>
        ///// Acumulado de aportes de recursos no exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de aportes de recursos no exentos")]
        //public decimal LiableContributtions { get; set; }

        ///// <summary>
        ///// Acumulado de rendimientos no exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de rendimientos no exentos")]
        //public decimal LiablePerformances { get; set; }

        ///// <summary>
        ///// Acumulado de gastos Bancarios no exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de gastos Bancarios no exentos")]
        //public decimal LiableBankCharges { get; set; }

        ///// <summary>
        ///// Acumulado de gravamen financiero descontado no exentos
        ///// </summary>
        //[JsonProperty(PropertyName = "Acumulado de gravamen financiero descontado no exentos")]
        //public new decimal LiableDiscountedCharge { get; set; }
        [JsonProperty(PropertyName = "Total de rendimientos generados")]
        public new decimal GeneratedPerformances { get; set; }
        [JsonProperty(PropertyName = "Provisión gravamen financiero")]
        public new decimal FinancialLienProvision { get; set; }
        [JsonProperty(PropertyName = "Total gastos bancarios")]
        public new decimal BankCharges { get; set; }
        [JsonProperty(PropertyName = "Total gravamen financiero descontado")]
        public new decimal DiscountedCharge { get; set; }
        [JsonProperty(PropertyName = "Rendimiento a incorporar")]
        public new decimal PerformancesToAdd { get; set; }
        [JsonIgnore]
        public bool? BuiltIn { get; set; } 
    }


    public class ManagedPerformancesOrderDto : EntryPerformanceOrder
    {
        [JsonProperty(PropertyName = "Estado")]
        public new string Status { get; set; }
        [JsonProperty(PropertyName = "Total de rendimientos generados")]
        public decimal GeneratedPerformances { get; set; }
        [JsonProperty(PropertyName = "Provisión gravamen financiero")]
        public decimal FinancialLienProvision { get; set; }
        [JsonProperty(PropertyName = "Total gastos bancarios")]
        public decimal BankCharges { get; set; }
        [JsonProperty(PropertyName = "Total gravamen financiero descontado")]
        public decimal DiscountedCharge { get; set; }
        [JsonProperty(PropertyName = "Rendimiento a incorporar")]
        public decimal PerformancesToAdd { get; set; }
    }


    public class ApprovedPerfomancesDto
    {
        [JsonProperty(PropertyName = "Cuenta Bancaria")]
        public string CuentaBancaria { get; set; }
        [JsonProperty(PropertyName = "Total de rendimientos generados")]
        public decimal TotalRendimientosGenerados { get; set; }
        [JsonProperty(PropertyName = "Rendimientos Incorporados")]
        public decimal Incorporados { get; set; }

        [JsonProperty(PropertyName = "Provisión gravamen financiero")]
        public decimal ProvisionGravamenFinanciero { get; set; }
        [JsonProperty(PropertyName = "Total gastos bancarios")]
        public decimal TotalGastosBancarios { get; set; }

        [JsonProperty(PropertyName = "Total gravamen financiero descontado")]
        public decimal TotalGravamenFinancieroDescontado { get; set; }
        [JsonProperty(PropertyName = "Visitas")]
        public decimal Visitas { get; set; }
        [JsonProperty(PropertyName = "Rendimiento a incorporar")]
        public decimal RendimientoIncorporar { get; set; }

    }


    public class DataResult
    {
        public string Numero { get; set; }

        public string Aportante { get; set; }

        public decimal? Anterior { get; set; }

        public decimal? Actual { get; set; }
    }


    public class MinuteTemplate
    {
        public string PerformancesDate { get; set; }
        public List<DataResult> Registers { get; set; }
    }
    
}
