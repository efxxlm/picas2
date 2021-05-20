﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FuenteFinanciacion
    {
        public FuenteFinanciacion()
        {
            AportanteFuenteFinanciacion = new HashSet<AportanteFuenteFinanciacion>();
            ComponenteUso = new HashSet<ComponenteUso>();
            ControlRecurso = new HashSet<ControlRecurso>();
            CuentaBancaria = new HashSet<CuentaBancaria>();
            GestionFuenteFinanciacion = new HashSet<GestionFuenteFinanciacion>();
            OrdenGiroDetalleDescuentoTecnicaAportante = new HashSet<OrdenGiroDetalleDescuentoTecnicaAportante>();
            OrdenGiroDetalleTerceroCausacionAportante = new HashSet<OrdenGiroDetalleTerceroCausacionAportante>();
            OrdenGiroDetalleTerceroCausacionDescuento = new HashSet<OrdenGiroDetalleTerceroCausacionDescuento>();
            ProyectoFuentes = new HashSet<ProyectoFuentes>();
            VigenciaAporte = new HashSet<VigenciaAporte>();
        }

        public int FuenteFinanciacionId { get; set; }
        public int AportanteId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public decimal? ValorFuente { get; set; }
        public int? CantVigencias { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? CofinanciacionDocumentoId { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual CofinanciacionDocumento CofinanciacionDocumento { get; set; }
        public virtual ICollection<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual ICollection<ComponenteUso> ComponenteUso { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
        public virtual ICollection<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual ICollection<GestionFuenteFinanciacion> GestionFuenteFinanciacion { get; set; }
        public virtual ICollection<OrdenGiroDetalleDescuentoTecnicaAportante> OrdenGiroDetalleDescuentoTecnicaAportante { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionAportante> OrdenGiroDetalleTerceroCausacionAportante { get; set; }
        public virtual ICollection<OrdenGiroDetalleTerceroCausacionDescuento> OrdenGiroDetalleTerceroCausacionDescuento { get; set; }
        public virtual ICollection<ProyectoFuentes> ProyectoFuentes { get; set; }
        public virtual ICollection<VigenciaAporte> VigenciaAporte { get; set; }
    }
}
