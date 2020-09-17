using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionParticipante
    {
        public SesionParticipante()
        {
<<<<<<< HEAD
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
            SesionComentario = new HashSet<SesionComentario>();
            SesionParticipanteVoto = new HashSet<SesionParticipanteVoto>();
            SesionSolicitudCompromiso = new HashSet<SesionSolicitudCompromiso>();
            SesionSolicitudObservacionProyecto = new HashSet<SesionSolicitudObservacionProyecto>();
            SesionSolicitudVoto = new HashSet<SesionSolicitudVoto>();
            SesionTemaVoto = new HashSet<SesionTemaVoto>();
<<<<<<< HEAD
            TemaCompromiso = new HashSet<TemaCompromiso>();
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
        }

        public int SesionParticipanteId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
<<<<<<< HEAD
        public bool? Eliminado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
=======

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual Usuario Usuario { get; set; }
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
        public virtual ICollection<SesionComentario> SesionComentario { get; set; }
        public virtual ICollection<SesionParticipanteVoto> SesionParticipanteVoto { get; set; }
        public virtual ICollection<SesionSolicitudCompromiso> SesionSolicitudCompromiso { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual ICollection<SesionSolicitudVoto> SesionSolicitudVoto { get; set; }
        public virtual ICollection<SesionTemaVoto> SesionTemaVoto { get; set; }
<<<<<<< HEAD
        public virtual ICollection<TemaCompromiso> TemaCompromiso { get; set; }
=======
>>>>>>> 44c6d1719f3208074f5544eb7da53e1ff00c009a
    }
}
