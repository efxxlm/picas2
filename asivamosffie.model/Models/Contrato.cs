using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contrato
    {
        public Contrato()
        {
            ContratoObservacion = new HashSet<ContratoObservacion>();
            ContratoPoliza = new HashSet<ContratoPoliza>();
        }

        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public DateTime FechaTramite { get; set; }
        public string TipoContratoCodigo { get; set; }
        public string NumeroContrato { get; set; }
        public string EstadoDocumentoCodigo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaEnvioFirma { get; set; }
        public DateTime FechaFirmaContratista { get; set; }
        public DateTime FechaFirmaFiduciaria { get; set; }
        public DateTime FechaFirmaContrato { get; set; }
        public string Observaciones { get; set; }
        public string RutaDocumento { get; set; }
        public string Objeto { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Plazo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual Contratacion Contratacion { get; set; }
        public virtual ICollection<ContratoObservacion> ContratoObservacion { get; set; }
        public virtual ICollection<ContratoPoliza> ContratoPoliza { get; set; }
    }
}
