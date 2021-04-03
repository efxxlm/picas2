using Newtonsoft.Json;

namespace asivamosffie.model.APIModels
{
    public class EntryPaymentOrder {

        [JsonProperty(PropertyName = "Estado")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "Número de orden de giro")]
        public string NumOrder { get; set; }

        [JsonProperty(PropertyName = "Fecha de pago")]
        public string PaymentDate { get; set; }
        [JsonProperty(PropertyName = "Impuestos")]
        public string Taxes { get; set; }

        [JsonProperty(PropertyName = "Valor neto girado")]
        public string NetValue { get; set; }

        //EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
        //NombreArchivo = pFile.FileName,
        //RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
        //RegistrosInvalidos = CantidadRegistrosInvalidos,
        //TotalRegistros = cantidadRegistrosTotales,
        //TipoCargue = typeFile,
        //FechaCargue = DateTime.Now,
    }
}
