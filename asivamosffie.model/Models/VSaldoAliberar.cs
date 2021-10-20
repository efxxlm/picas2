using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSaldoAliberar
    {
        public string NumeroDrp { get; set; }
        public int? ComponenteUsoId { get; set; }
        public int? ComponenteUsoNovedadId { get; set; }
        public string CodigoUso { get; set; }
        public decimal? ValorUso { get; set; }
        public string NombreUso { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int? NovedadContractualRegistroPresupuestalId { get; set; }
        public int ContratoId { get; set; }
        public string LlaveMen { get; set; }
        public decimal ValorSolicitud { get; set; }
        public int? ComponenteUsoHistoricoId { get; set; }
        public int? ComponenteUsoNovedadHistoricoId { get; set; }
        public decimal? ValorLiberar { get; set; }
        public int RegistroCompleto { get; set; }
        public decimal? SaldoTesoral { get; set; }
        public bool? EsNovedad { get; set; }
    }
}
