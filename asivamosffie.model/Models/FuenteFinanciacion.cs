using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace asivamosffie.model.Models
{
    public partial class FuenteFinanciacion
    {
        public FuenteFinanciacion()
        {
            AportanteFuenteFinanciacion = new HashSet<AportanteFuenteFinanciacion>();
            ControlRecurso = new HashSet<ControlRecurso>();
            CuentaBancaria = new HashSet<CuentaBancaria>();
            GestionFuenteFinanciacion = new HashSet<GestionFuenteFinanciacion>();
            VigenciaAporte = new HashSet<VigenciaAporte>();
        }

        public int FuenteFinanciacionId { get; set; }
        public int AportanteId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public decimal ValorFuente { get; set; }
        public int CantVigencias { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        [NotMapped]
        public decimal ValorAporteEnCuenta
        {
            get
            {
                try
                {
                    
                    return this.ControlRecurso.Sum(e => (decimal)e.ValorConsignacion);
                }
                catch
                {
                    throw new Exception("Error calculado en valor aporte en cuenta");
                }
            }
        }
        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual ICollection<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
        public virtual ICollection<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual ICollection<GestionFuenteFinanciacion> GestionFuenteFinanciacion { get; set; }
        public virtual ICollection<VigenciaAporte> VigenciaAporte { get; set; }
    }
}
