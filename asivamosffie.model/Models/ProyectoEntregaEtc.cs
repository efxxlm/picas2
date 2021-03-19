using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoEntregaEtc
    {
        public ProyectoEntregaEtc()
        {
            RepresentanteEtcrecorrido = new HashSet<RepresentanteEtcrecorrido>();
        }

        public int ProyectoEntregaEtcid { get; set; }
        public int InformeFinalId { get; set; }
        public DateTime? FechaRecorridoObra { get; set; }
        public int? NumRepresentantesRecorrido { get; set; }
        public DateTime? FechaEntregaDocumentosEtc { get; set; }
        public string NumRadicadoDocumentosEntregaEtc { get; set; }
        public DateTime? FechaFirmaActaBienesServicios { get; set; }
        public string ActaBienesServicios { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaFirmaActaEngregaFisica { get; set; }
        public string UrlActaEntregaFisica { get; set; }
        public bool? RegistroCompletoActaBienesServicios { get; set; }
        public bool? RegistroCompletoRecorridoObra { get; set; }
        public bool? RegistroCompletoRemision { get; set; }

        public virtual InformeFinal InformeFinal { get; set; }
        public virtual ICollection<RepresentanteEtcrecorrido> RepresentanteEtcrecorrido { get; set; }
    }
}
