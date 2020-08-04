using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestal
    {
        public DisponibilidadPresupuestal()
        {
            DisponibilidadPresupuestalProyecto = new HashSet<DisponibilidadPresupuestalProyecto>();
        }

        public int DisponibilidadPresupuestalId { get; set; }
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime FechaSolicitud { get; set; }
        [Required]
        [StringLength(100)]
        public string TipoSolicitudCodigo { get; set; }
        [Required]
        [StringLength(50)]
        public string NumeroSolicitud { get; set; }
        [Required]
        [StringLength(100)]
        public string OpcionContratarCodigo { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal ValorSolicitud { get; set; }

        [StringLength(100)]
        public string EstadoSolicitudCodigo { get; set; }
        [Required]
        [StringLength(1000)]
        public string Objeto { get; set; }
        public bool Eliminado { get; set; }
        [Column("FechaDDP", TypeName = "datetime")]
        public DateTime? FechaDdp { get; set; }
        [Column("NumeroDDP")]
        [StringLength(200)]
        public string NumeroDdp { get; set; }
        [Column("RutaDDP")]
        [StringLength(300)]
        public string RutaDdp { get; set; }
        [StringLength(2000)]
        public string Observacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }

        public bool? RegistroCompleto { get; set; }

        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }
        [StringLength(200)]
        public string UsuarioModificacion { get; set; }

        [InverseProperty("DisponibilidadPresupuestal")]
        public virtual ICollection<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
    }
}
