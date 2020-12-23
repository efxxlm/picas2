using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class ListConcecutivoProyectoAdministrativo
    {
        public int ProyectoId { get; set; }
        public string Concecutivo {get; set;}
        public Dominio ListaAportantes { get; set; }
        public List<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public string NombreAportante { get; set; }
    }
}
