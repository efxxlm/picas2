using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.AditionalModels
{
    public class RegistroAnticipo
    {
        public int ContratacionProyectoId { get; set; }
        public double? ValorAnticipo { get; set; }
        public double? ValorAnticipoDesembolsado { get; set; }
        public double? DiferenciaCorrespondienteImpuestos { get; set; }
        public int? TotalGirosSolicitadosAprobados { get; set; }
        public string SaldoAnticipoPorSolicitar { get; set; }
        public IEnumerable<RegistroIndividualAnticipo> RegistroIndividualAnticipo { get; set; }
    }
    public class RegistroIndividualAnticipo
    {
        public double? ValorAmortizacion { get; set; }
        public double? SaldoAmortizar { get; set; }
    }
}
