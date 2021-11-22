using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ArchivoCargue
    {
        [NotMapped]
        public string estadoCargue  { get; set; }

        [NotMapped]
        public AjustePragramacionObservacion TempAjustePragramacionObservacion { get; set; }

    }
}
