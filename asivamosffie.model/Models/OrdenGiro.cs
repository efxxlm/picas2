using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiro
    {
        public OrdenGiro()
        {
            BalanceFinancieroTrasladoValor = new HashSet<BalanceFinancieroTrasladoValor>();
            OrdenGiroDetalle = new HashSet<OrdenGiroDetalle>();
            OrdenGiroObservacion = new HashSet<OrdenGiroObservacion>();
            OrdenGiroPago = new HashSet<OrdenGiroPago>();
            OrdenGiroTercero = new HashSet<OrdenGiroTercero>();
            SolicitudPago = new HashSet<SolicitudPago>();
        }

        public int OrdenGiroId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public DateTime? FechaRegistroCompleto { get; set; }
        public bool? RegistroCompletoVerificar { get; set; }
        public DateTime? FechaRegistroCompletoVerificar { get; set; }
        public bool? RegistroCompletoAprobar { get; set; }
        public DateTime? FechaRegistroCompletoAprobar { get; set; }
        public bool? RegistroCompletoTramitar { get; set; }
        public DateTime? FechaRegistroCompletoTramitar { get; set; }
        public bool? TieneObservacion { get; set; }
        public string ConsecutivoOrigen { get; set; }
        public string UrlSoporteFirmadoVerificar { get; set; }
        public string UrlSoporteFirmadoAprobar { get; set; }
        public decimal? ValorNetoGiro { get; set; }
        public bool? TieneTraslado { get; set; }
        public decimal? ValorNetoGiroTraslado { get; set; }

        public virtual ICollection<BalanceFinancieroTrasladoValor> BalanceFinancieroTrasladoValor { get; set; }
        public virtual ICollection<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
        public virtual ICollection<OrdenGiroObservacion> OrdenGiroObservacion { get; set; }
        public virtual ICollection<OrdenGiroPago> OrdenGiroPago { get; set; }
        public virtual ICollection<OrdenGiroTercero> OrdenGiroTercero { get; set; }
        public virtual ICollection<SolicitudPago> SolicitudPago { get; set; }
    }
}
