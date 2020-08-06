using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TempOrdenLegibilidad
    {
        public int TempId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? TipoProponenteId { get; set; }
        public string NombreProponente { get; set; }
        public string NumeroIddentificacionProponente { get; set; }
        public string Departamento { get; set; }
        public string Minicipio { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string NombreEntidad { get; set; }
        public string IdentificacionTributaria { get; set; }
        public string RepresentanteLegal { get; set; }
        public string CedulaRepresentanteLegal { get; set; }
        public string DepartamentoRl { get; set; }
        public string MunucipioRl { get; set; }
        public string Legal { get; set; }
        public string DireccionRl { get; set; }
        public string TelefonoRl { get; set; }
        public string CorreoRl { get; set; }
        public string NombreOtoConsorcio { get; set; }
        public string EntiddaesQueIntegranLaUnionTemporal { get; set; }
        public string NombreIntegrante { get; set; }
        public string PorcentajeParticipacion { get; set; }
        public string NombreRlutoConsorcio { get; set; }
        public string CcrlutoConsorcio { get; set; }
        public string DepartamentoRlutoConsorcio { get; set; }
        public string MinicipioRlutoConsorcio { get; set; }
        public string DireccionRlutoConsorcio { get; set; }
        public string TelefonoRlutoConsorcio { get; set; }
        public string CorreoRlutoConsorcio { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
