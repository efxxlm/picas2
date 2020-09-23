﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPerfil
    {
        public ContratoPerfil()
        {
            ContratoPerfilObservacion = new HashSet<ContratoPerfilObservacion>();
        }

        public int ContratoPerfilId { get; set; }
        public int ContratoId { get; set; }
        public string PerfilCodigo { get; set; }
        public int CantidadHvRequeridas { get; set; }
        public int CantidadHvRecibidas { get; set; }
        public int CantidadHvAprobadas { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroRadicadoFfie { get; set; }
        public string NumeroRadicadoFfie1 { get; set; }
        public string NumeroRadicadoFfie2 { get; set; }
        public string NumeroRadicadoFfie3 { get; set; }
        public string RutaSoporte { get; set; }
        public bool? ConObervacionesSupervision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
<<<<<<< HEAD
        public bool Eliminado { get; set; }
        public bool RegistroCompleto { get; set; }
=======
>>>>>>> origin/3.4.4-Generar-polizas-y-garantias

        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<ContratoPerfilObservacion> ContratoPerfilObservacion { get; set; }
    }
}
