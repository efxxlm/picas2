﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TemaCompromiso
    {
        public TemaCompromiso()
        {
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
        }

        public int TemaCompromisoId { get; set; }
        public int SesionTemaId { get; set; }
        public string Tarea { get; set; }
        public string Responsable { get; set; }
        public DateTime FechaCumplimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SesionComiteTema SesionTema { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
    }
}
