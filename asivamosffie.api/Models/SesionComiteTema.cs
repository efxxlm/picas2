﻿using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SesionComiteTema
    {
        public SesionComiteTema()
        {
            SesionTemaVoto = new HashSet<SesionTemaVoto>();
            TemaCompromiso = new HashSet<TemaCompromiso>();
        }

        public int SesionTemaId { get; set; }
        public string Tema { get; set; }
        public string ResponsableCodigo { get; set; }
        public int? TiempoIntervencion { get; set; }
        public string RutaSoporte { get; set; }
        public string Observaciones { get; set; }
        public bool? EsAprobado { get; set; }
        public string ObservacionesDecision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? ComiteTecnicoId { get; set; }
        public bool? EsProposicionesVarios { get; set; }
        public bool? RequiereVotacion { get; set; }
        public string EstadoTemaCodigo { get; set; }
        public bool? GeneraCompromiso { get; set; }
        public int? CantCompromisos { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual ICollection<SesionTemaVoto> SesionTemaVoto { get; set; }
        public virtual ICollection<TemaCompromiso> TemaCompromiso { get; set; }
    }
}
