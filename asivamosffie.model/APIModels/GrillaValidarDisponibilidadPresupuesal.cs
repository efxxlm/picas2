using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{

    

    public class GrillaValidarDisponibilidadPresupuesal
    {
        public int Id { get; set; } // id Registro
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitudText { get; set; }
        public bool EstadoRegistro { get; set; }
        public string EstadoRegistroText { get; set; }
    }

    public class DetailValidarDisponibilidadPresupuesal
    {
        public int Id { get; set; } // id Registro
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitudText { get; set; }
        public string OpcionPorContratar { get; set; }
        public string NumeroSolicitud { get; set; }
        public string NumeroDDP { get; set; } // de proyectos
        public decimal ValorSolicitud { get; set; }
        public string RubroPorFinanciar { get; set; }
        public string Objeto { get; set; }
        public DateTime? FechaComiteTecnico { get; set; } // de proyectos
        public string NumeroComite { get; set; } // de proyectos
        public string LlameMEN { get; set; }
        public string TipoProyecto { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }

        //Aportante
        public string TipoAportante { get; set; }
        public string NombreAportante { get; set; }
        public decimal ValorAportanteProyecto { get; set; }
        //Fuentes
        public string FuenteNombre { get; set; }
        public decimal SaldoActualFuente { get; set; }
        public decimal ValorSolcitadoFuente { get; set; }
        public decimal NUmeroSaldoFuente { get; set; }
        public string EstadosDeLasFuentes { get; set; }
        public string Observaciones { get; set; }
        public StringBuilder htmlContent { get; set; }
    }
}
