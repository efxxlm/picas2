using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class RequisitoTecnicoRadicado
    {
        public int RequisitoTecnicoRadicado1 { get; set; }
        public int ProyectoRequisitoTecnicoId { get; set; }
        public string NumeroRadicadoFfie { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual ProyectoRequisitoTecnico ProyectoRequisitoTecnico { get; set; }
    }
}
