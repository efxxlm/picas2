using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoRequisitoTecnico
    {
        public ProyectoRequisitoTecnico()
        {
            RequisitoTecnicoRadicado = new HashSet<RequisitoTecnicoRadicado>();
        }

        public int ProyectoRequisitoTecnicoId { get; set; }
        public int ProyectoId { get; set; }
        public string PerfilCodigo { get; set; }
        public int CantidadHojasDeVida { get; set; }
        public int CantidadRecibidas { get; set; }
        public int CantidadAprobadas { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public string Observacion { get; set; }
        public string DireccionUrl { get; set; }
        public string EstadoRequisitoCodigo { get; set; }
        public bool Estado { get; set; }
        public string EstadoHojasDeVidaCodigo { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<RequisitoTecnicoRadicado> RequisitoTecnicoRadicado { get; set; }
    }
}
