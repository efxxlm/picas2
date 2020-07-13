using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionCotizacion
    {
        [Key]
        public int ProcesoSeleccionCotizacionId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        [Required]
        [StringLength(20)]
        public string NombreOrganizacion { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal ValorCotizacion { get; set; }
        [Required]
        [StringLength(3000)]
        public string Descripcion { get; set; }
        [StringLength(500)]
        public string UrlSoporte { get; set; }
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
        [InverseProperty("ProcesoSeleccionCotizacion")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
