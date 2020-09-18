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
        public List<CofinanciacionAportante> ListaAportantes { get; set; }

    }
}
