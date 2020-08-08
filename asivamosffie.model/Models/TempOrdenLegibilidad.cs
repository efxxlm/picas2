using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class TempOrdenLegibilidad
    {
        [Key]
        public int TempOrdenLegibilidadId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaCreacion { get; set; }
        public int? TipoProponenteId { get; set; }
        [StringLength(200)]
        public string NombreProponente { get; set; }
        [StringLength(200)]
        public string NumeroIddentificacionProponente { get; set; }
        public int Departamento { get; set; }
        public int Minicipio { get; set; }
        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }
        [Required]
        [StringLength(200)]
        public string Telefono { get; set; }
        [Required]
        [StringLength(200)]
        public string Correo { get; set; }
        [StringLength(500)]
        public string NombreEntidad { get; set; }
        public int IdentificacionTributaria { get; set; }
        [Required]
        [StringLength(500)]
        public string RepresentanteLegal { get; set; }
        public int CedulaRepresentanteLegal { get; set; }
        [Column("DepartamentoRL")]
        public int DepartamentoRl { get; set; }
        [Column("MunucipioRL")]
        public int MunucipioRl { get; set; }
        [StringLength(200)]
        public string Legal { get; set; }
        [Required]
        [Column("DireccionRL")]
        [StringLength(200)]
        public string DireccionRl { get; set; }
        [Required]
        [Column("TelefonoRL")]
        [StringLength(200)]
        public string TelefonoRl { get; set; }
        [Required]
        [Column("CorreoRL")]
        [StringLength(200)]
        public string CorreoRl { get; set; }
        [Required]
        [Column("NombreOToConsorcio")]
        [StringLength(500)]
        public string NombreOtoConsorcio { get; set; }
        public int? EntiddaesQueIntegranLaUnionTemporal { get; set; }
        [Required]
        [StringLength(200)]
        public string NombreIntegrante { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal PorcentajeParticipacion { get; set; }
        [Required]
        [Column("NombreRLUToConsorcio")]
        [StringLength(1000)]
        public string NombreRlutoConsorcio { get; set; }
        [Column("CCRLUToConsorcio")]
        public int CcrlutoConsorcio { get; set; }
        [Column("DepartamentoRLUToConsorcio")]
        public int DepartamentoRlutoConsorcio { get; set; }
        [Column("MinicipioRLUToConsorcio")]
        public int MinicipioRlutoConsorcio { get; set; }
        [Required]
        [Column("DireccionRLUToConsorcio")]
        [StringLength(500)]
        public string DireccionRlutoConsorcio { get; set; }
        [Column("TelefonoRLUToConsorcio")]
        [StringLength(200)]
        public string TelefonoRlutoConsorcio { get; set; }
        [Required]
        [Column("CorreoRLUToConsorcio")]
        [StringLength(1000)]
        public string CorreoRlutoConsorcio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }
        [StringLength(200)]
        public string UsuarioModificacion { get; set; }
    }
}
