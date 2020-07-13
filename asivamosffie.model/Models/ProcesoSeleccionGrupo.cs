using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionGrupo
    {
        [Key]
        public int ProcesoSeleccionGrupoId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        [Required]
        [StringLength(500)]
        public string NombreGrupo { get; set; }
        [Required]
        [StringLength(100)]
        public string TipoPresupuestoCodigo { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? Valor { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? ValorMinimoCategoria { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? ValorMaximoCategoria { get; set; }
        public int PlazoMeses { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }
        [StringLength(200)]
        public string UsuarioModificacion { get; set; }

        [ForeignKey(nameof(ProcesoSeleccionId))]
        [InverseProperty("ProcesoSeleccionGrupo")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
