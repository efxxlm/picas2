﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Menu
    {
        public Menu()
        {
            LiquidacionContratacionObservacion = new HashSet<LiquidacionContratacionObservacion>();
            MensajesValidaciones = new HashSet<MensajesValidaciones>();
            MenuPerfil = new HashSet<MenuPerfil>();
            OrdenGiroObservacion = new HashSet<OrdenGiroObservacion>();
            SolicitudPagoObservacion = new HashSet<SolicitudPagoObservacion>();
        }

        public int MenuId { get; set; }
        public int? MenuPadreId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Posicion { get; set; }
        public string Icono { get; set; }
        public string RutaFormulario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<LiquidacionContratacionObservacion> LiquidacionContratacionObservacion { get; set; }
        public virtual ICollection<MensajesValidaciones> MensajesValidaciones { get; set; }
        public virtual ICollection<MenuPerfil> MenuPerfil { get; set; }
        public virtual ICollection<OrdenGiroObservacion> OrdenGiroObservacion { get; set; }
        public virtual ICollection<SolicitudPagoObservacion> SolicitudPagoObservacion { get; set; }
    }
}
