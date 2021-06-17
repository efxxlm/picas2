using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CarguePago
    {
        public CarguePago()
        {
            OrdenGiroPago = new HashSet<OrdenGiroPago>();
        }

        public int CarguePagoId { get; set; }
        public string NombreArchivo { get; set; }
        public string JsonContent { get; set; }
        public string Observaciones { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosValidos { get; set; }
        public int RegistrosInvalidos { get; set; }
        public DateTime FechaCargue { get; set; }
        public bool CargueValido { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Errores { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<OrdenGiroPago> OrdenGiroPago { get; set; }
    }
}
