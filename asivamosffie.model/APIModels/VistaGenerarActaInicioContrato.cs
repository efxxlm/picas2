﻿using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class VistaGenerarActaInicioContrato
    {
        //public string FechaAprobacionRequisitos { get; set; }
        public string NumeroContrato { get; set; }
        public string VigenciaContrato { get; set; }
        public string FechaFirmaContrato { get; set; }
        public string NumeroDRP1 { get; set; }
        public string FechaGeneracionDRP1 { get; set; }
        public string NumeroDRP2 { get; set; }
        public string FechaGeneracionDRP2 { get; set; }
        public string FechaAprobacionGarantiaPoliza { get; set; }
        public string Objeto { get; set; }
        public string ValorInicialContrato { get; set; }
        public string ValorActualContrato { get; set; }
        public string ValorFase1Preconstruccion { get; set; }
        public string Valorfase2ConstruccionObra { get; set; }
        public string NumeroIdentificacionRepresentanteContratistaInterventoria { get; set; }
        public string NombreRepresentanteContratistaInterventoria { get; set; }
        public string NumeroIdentificacionContratistaInterventoria { get; set; }
        public string NombreEntidadContratistaSupervisorInterventoria { get; set; }
        public string? PlazoInicialContratoSupervisor { get; set; }

        public Int32? PlazoFase1PreMeses { get; set; }
        public Int32? PlazoFase2ConstruccionDias { get; set; }
        
        public Int32? PlazoFase1PreDias { get; set; }
        public Int32? PlazoFase2ConstruccionMeses { get; set; }                           

        public string NumeroIdentificacionRepresentanteContratistaObraInterventoria { get; set; }
        public string NombreRepresentanteContratistaObra { get; set; }
        public string NumeroIdentificacionEntidadContratistaObra { get; set; }
        public string NombreEntidadContratistaObra { get; set; }

        //datos PDF opcionales
        public string? LlaveMENContrato { get; set; }
        public string? InstitucionEducativaLlaveMEN { get; set; }
        public string? DepartamentoYMunicipioLlaveMEN { get; set; }        

        public string? ObservacionOConsideracionesEspeciales { get; set; }

        public string? FechaPrevistaTerminacion { get; set; }

        public string? FechaActaInicio { get; set; }

        public DateTime? FechaActaInicioDateTime { get; set; }

        public DateTime? FechaPrevistaTerminacionDateTime { get; set; }              

        public Int32? CantidadProyectosAsociados { get; set; }


    }
}
