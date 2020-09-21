using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GridComiteTecnicoCompromiso
    {
        public int SesionComiteTecnicoCompromisoId { get; set; }
        public DateTime FechaCumplimiento { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoCompromiso { get; set; }

        public string Tarea { get; set;}
        public string Responzable { get; set;}
        public DateTime FechaReporte { get; set;}
        public string EstadoReporte { get; set;}

        public ICollection<ComiteTecnico> ComiteTecnicoLista { get; set; }
    }
}
