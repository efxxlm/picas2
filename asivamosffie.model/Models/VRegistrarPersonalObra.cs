using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarPersonalObra
    {
        public int ContratacionProyectoId { get; set; }
        public DateTime? FechaFirmaActaInicio { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoIntervencion { get; set; }
        public string Sede { get; set; }
        public string InstitucionEducativa { get; set; }
        public string EstadoProgramacionInicialCodigo { get; set; }
        public string EstadoProgramacionInicial { get; set; }
    }
}
