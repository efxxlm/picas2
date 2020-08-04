using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestalProyecto
    {
        [Key]
        public int DisponibilidadPresupuestalProyectoId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public int ProyectoId { get; set; }
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaCreacion { get; set; }
        [StringLength(200)]
        public string UsuarioModificacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
