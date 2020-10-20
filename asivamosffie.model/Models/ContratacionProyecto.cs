﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyecto
    {
        public ContratacionProyecto()
        {
            ContratacionProyectoAportante = new HashSet<ContratacionProyectoAportante>();
            DefensaJudicialContratacionProyecto = new HashSet<DefensaJudicialContratacionProyecto>();
            SesionSolicitudObservacionProyecto = new HashSet<SesionSolicitudObservacionProyecto>();
        }

        public int ContratacionProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Eliminado { get; set; }
        public bool? EsReasignacion { get; set; }
        public string PorcentajeAvanceObra { get; set; }
        public bool? RequiereLicencia { get; set; }
        public bool? LicenciaVigente { get; set; }
        public string NumeroLicencia { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Activo { get; set; }
        public bool? EsAvanceobra { get; set; }
        public bool? TieneMonitoreoWeb { get; set; }
        public string EstadoRequisitosVerificacionCodigo { get; set; }
        public DateTime? FechaAprobacionRequisitos { get; set; }
        public bool? RegistroCompleto { get; set; }
         
        public virtual Contratacion Contratacion { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual ICollection<DefensaJudicialContratacionProyecto> DefensaJudicialContratacionProyecto { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
    }
}
