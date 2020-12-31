using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class VistaContratoProyectos
    {
        public Int32 ProyectoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NombreContratista { get; set; }
        public Int32 NumeroProyectosAsociados { get; set; }

       public List<ProyectoGrilla> lstProyectoGrilla { get; set; }
        public int Semaforo { get; set; }
    }
        

}
