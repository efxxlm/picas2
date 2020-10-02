using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.PostParameters
{
    public class PostDDPRequestParameter
    {
        public int DisponibilidadPresupuestalId { get; set; }
        public string TipoSolicitudDDPEspecial { get; set; }
        public string Objeto { get; set; }
        public string  NumeroRadicadoSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Eliminado { get; set; }
    }
}
