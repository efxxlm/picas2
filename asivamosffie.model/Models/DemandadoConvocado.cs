using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DemandadoConvocado
    {
        public int DemandadoConvocadoId { get; set; }
        public bool? EsConvocado { get; set; }
        public string Nombre { get; set; }
        public string TipoIdentificacionCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string ConvocadoAutoridadDespacho { get; set; }
        public int? LocalizacionIdMunicipio { get; set; }
        public string RadicadoDespacho { get; set; }
        public DateTime? FechaRadicado { get; set; }
        public string MedioControlAccion { get; set; }
        public string EtapaProcesoFfiecodigo { get; set; }
        public DateTime? CaducidadPrescripcion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? DefensaJudicialId { get; set; }

        public virtual DefensaJudicial DefensaJudicial { get; set; }
    }
}
