using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contrato
    {
        public Contrato()
        {
            ContratoConstruccion = new HashSet<ContratoConstruccion>();
            ContratoObservacion = new HashSet<ContratoObservacion>();
            ContratoPerfil = new HashSet<ContratoPerfil>();
        }

        public int ContratacionId { get; set; }
        public DateTime? FechaTramite { get; set; }
        public string TipoContratoCodigo { get; set; }
        public string NumeroContrato { get; set; }
        public string EstadoDocumentoCodigo { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaEnvioFirma { get; set; }
        public DateTime? FechaFirmaContratista { get; set; }
        public DateTime? FechaFirmaFiduciaria { get; set; }
        public DateTime? FechaFirmaContrato { get; set; }
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
        public int? CantidadPerfiles { get; set; }
        public string EstadoVerificacionCodigo { get; set; }
        public string EstadoFase1 { get; set; }
        public string EstadoActa { get; set; }
        public DateTime? FechaActaInicioFase1 { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        public int? PlazoFase1PreMeses { get; set; }
        public int? PlazoFase1PreDias { get; set; }
        public int? PlazoFase2ConstruccionMeses { get; set; }
        public int? PlazoFase2ConstruccionDias { get; set; }
        public bool? ConObervacionesActa { get; set; }
        public DateTime? FechaFirmaActaContratista { get; set; }
        public DateTime? FechaFirmaActaContratistaInterventoria { get; set; }
        public string RutaActa { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? ConObervacionesActaFase1 { get; set; }
        public DateTime? FechaFirmaActaContratistaFase1 { get; set; }
        public DateTime? FechaFirmaActaContratistaInterventoriaFase1 { get; set; }
        public string RutaActaFase1 { get; set; }
        public DateTime? FechaActaInicioFase2 { get; set; }
        public DateTime? FechaTerminacionFase2 { get; set; }
        public DateTime? FechaFirmaActaContratistaFase2 { get; set; }
        public DateTime? FechaFirmaActaContratistaInterventoriaFase2 { get; set; }
        public string RutaActaFase2 { get; set; }
        public string RutaActaSuscrita { get; set; }
        public int ContratoId { get; set; }

        public virtual Contratacion Contratacion { get; set; }
        public virtual ICollection<ContratoConstruccion> ContratoConstruccion { get; set; }
        public virtual ICollection<ContratoObservacion> ContratoObservacion { get; set; }
        public virtual ICollection<ContratoPerfil> ContratoPerfil { get; set; }
    }
}
