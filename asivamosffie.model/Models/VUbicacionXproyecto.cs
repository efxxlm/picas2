using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VUbicacionXproyecto
    {
        public int ProyectoId { get; set; }
        public string MunicipioId { get; set; }
        public string Municipio { get; set; }
        public string DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public string RegionId { get; set; }
        public string Region { get; set; }
    }
}
