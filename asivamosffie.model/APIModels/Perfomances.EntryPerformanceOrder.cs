using Newtonsoft.Json;

namespace asivamosffie.model.APIModels
{
    public class EntryPerformanceOrder
    {
        [JsonProperty(PropertyName = "Estado")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "Fecha de rendimientos")]
        public string PerformancesDate { get; set; }

        [JsonProperty(PropertyName = "Número de Cuenta")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Acumulado de aportes de recursos exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de aportes de recursos exentos")]

        public decimal ExemptResources { get; set; }

        /// <summary>
        /// Acumulado de rendimientos exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de rendimientos exentos")]
        public decimal ExemptPerformances { get; set; }

        /// <summary>
        /// Acumulado de gastos Bancarios exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de gastos Bancarios exentos")]
        public decimal ExemptBankCharges { get; set; }

        /// <summary>
        /// Acumulado de gravamen financiero descontado exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de gravamen financiero descontado exentos")]
        public decimal ExemptDiscountedCharge { get; set; }

        /// <summary>
        /// Acumulado de aportes de recursos no exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de aportes de recursos no exentos")]
        public decimal LiableContributtions { get; set; }

        /// <summary>
        /// Acumulado de rendimientos no exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de rendimientos no exentos")]
        public decimal LiablePerformances { get; set; }

        /// <summary>
        /// Acumulado de gastos Bancarios no exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de gastos Bancarios no exentos")]
        public decimal LiableBankCharges { get; set; }

        /// <summary>
        /// Acumulado de gravamen financiero descontado no exentos
        /// </summary>
        [JsonProperty(PropertyName = "Acumulado de gravamen financiero descontado no exentos")]
        public decimal LiableDiscountedCharge { get; set; }

    }
}
