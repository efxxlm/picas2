using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contratacion
    {
        public Contratacion()
        {
            ContratacionObservacion = new HashSet<ContratacionObservacion>();
            ContratacionProyecto = new HashSet<ContratacionProyecto>();
            Contrato = new HashSet<Contrato>();
<<<<<<< HEAD
            DisponibilidadPresupuestal = new HashSet<DisponibilidadPresupuestal>();
=======
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
        }

        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public int? ContratistaId { get; set; }
        public bool? EsObligacionEspecial { get; set; }
        public string ConsideracionDescripcion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaEnvioDocumentacion { get; set; }
        public string Observaciones { get; set; }
        public string RutaMinuta { get; set; }
        public DateTime? FechaTramite { get; set; }
        public bool? Estado { get; set; }
        public bool? EsMultiProyecto { get; set; }
        public string TipoContratacionCodigo { get; set; }
<<<<<<< HEAD
        public bool? RegistroCompleto { get; set; }
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a

        public virtual Contratista Contratista { get; set; }
        public virtual ICollection<ContratacionObservacion> ContratacionObservacion { get; set; }
        public virtual ICollection<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual ICollection<Contrato> Contrato { get; set; }
<<<<<<< HEAD
        public virtual ICollection<DisponibilidadPresupuestal> DisponibilidadPresupuestal { get; set; }
=======
        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
    }
}
