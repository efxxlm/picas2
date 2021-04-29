﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CarguePagosRendimientos
    {
        public CarguePagosRendimientos()
        {
            OrdenGiroPago = new HashSet<OrdenGiroPago>();
            RendimientosIncorporados = new HashSet<RendimientosIncorporados>();
        }

        public int CargaPagosRendimientosId { get; set; }
        public string NombreArchivo { get; set; }
        public string Json { get; set; }
        public string Observaciones { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosValidos { get; set; }
        public int RegistrosInvalidos { get; set; }
        public string EstadoCargue { get; set; }
        public string TipoCargue { get; set; }
        public DateTime FechaCargue { get; set; }
        public DateTime? FechaTramite { get; set; }
        public string TramiteJson { get; set; }
        public int RegistrosConsistentes { get; set; }
        public int RegistrosInconsistentes { get; set; }
        public bool PendienteAprobacion { get; set; }
        public bool MostrarInconsistencias { get; set; }
        public bool Eliminado { get; set; }
        public bool CargueValido { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Errores { get; set; }
        public DateTime? FechaActa { get; set; }
        public string RutaActa { get; set; }

        public virtual ICollection<OrdenGiroPago> OrdenGiroPago { get; set; }
        public virtual ICollection<RendimientosIncorporados> RendimientosIncorporados { get; set; }
    }
}
