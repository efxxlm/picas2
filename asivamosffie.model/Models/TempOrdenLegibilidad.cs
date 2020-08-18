using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TempOrdenLegibilidad
    {
        public int TempOrdenLegibilidadId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? TipoProponenteId { get; set; }
        public string NombreProponente { get; set; }
        public string NumeroIddentificacionProponente { get; set; }
        public int Departamento { get; set; }
        public int Minicipio { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string NombreEntidad { get; set; }
        public int IdentificacionTributaria { get; set; }
        public string RepresentanteLegal { get; set; }
        public int CedulaRepresentanteLegal { get; set; }
        public int DepartamentoRl { get; set; }
        public int MunucipioRl { get; set; }
        public string Legal { get; set; }
        public string DireccionRl { get; set; }
        public string TelefonoRl { get; set; }
        public string CorreoRl { get; set; }
        public string NombreOtoConsorcio { get; set; }
        public int? EntiddaesQueIntegranLaUnionTemporal { get; set; }
        public string NombreIntegrante { get; set; }
        public decimal PorcentajeParticipacion { get; set; }
        public string NombreRlutoConsorcio { get; set; }
        public int CcrlutoConsorcio { get; set; }
        public int DepartamentoRlutoConsorcio { get; set; }
        public int MinicipioRlutoConsorcio { get; set; }
        public string DireccionRlutoConsorcio { get; set; }
        public string TelefonoRlutoConsorcio { get; set; }
        public string CorreoRlutoConsorcio { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ArchivoCargue ArchivoCargue { get; set; }
    }
}
