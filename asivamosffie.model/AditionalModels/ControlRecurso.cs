
using System.ComponentModel.DataAnnotations.Schema;
namespace asivamosffie.model.Models
{
    public partial class ControlRecurso
    {
        [NotMapped]
        public CofinanciacionDocumento CofinanciacionDocumento { get; set; }

        [NotMapped]
        public VigenciaAporte VigenciaAporte { get; set; }
    }
}
