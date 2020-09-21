using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class ListAdminProyect
    {
        public int ProyectoId { get; set; }
        public decimal ValorAporte { get; set; }
        public int AportanteId { get; set; }
        public decimal ValorFuente { get; set; }
        public int NombreAportanteId { get; set; }
        public string NombreAportante { get; set; }
        public int FuenteAportanteId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public string NombreFuente { get; set; }
    }
}
