using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionParticipante
    {
        public SesionParticipante()
        {
<<<<<<< HEAD
=======
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
            SesionComentario = new HashSet<SesionComentario>();
            SesionParticipanteVoto = new HashSet<SesionParticipanteVoto>();
            SesionSolicitudCompromiso = new HashSet<SesionSolicitudCompromiso>();
            SesionSolicitudObservacionProyecto = new HashSet<SesionSolicitudObservacionProyecto>();
            SesionSolicitudVoto = new HashSet<SesionSolicitudVoto>();
            SesionTemaVoto = new HashSet<SesionTemaVoto>();
        }

        public int SesionParticipanteId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
<<<<<<< HEAD

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Usuario Usuario { get; set; }
=======
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
>>>>>>> 3.3.2_Validar-disponibilidad-de-presupuesto-para-ejecución-de-proyecto
        public virtual ICollection<SesionComentario> SesionComentario { get; set; }
        public virtual ICollection<SesionParticipanteVoto> SesionParticipanteVoto { get; set; }
        public virtual ICollection<SesionSolicitudCompromiso> SesionSolicitudCompromiso { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual ICollection<SesionSolicitudVoto> SesionSolicitudVoto { get; set; }
        public virtual ICollection<SesionTemaVoto> SesionTemaVoto { get; set; }
    }
}
