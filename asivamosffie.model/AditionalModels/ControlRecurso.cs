﻿
using System.ComponentModel.DataAnnotations.Schema;
namespace asivamosffie.model.Models
{
    public partial class ControlRecurso
    {
        [NotMapped]
        public CofinanciacionDocumento VigenciaAporte { get; set; }

    }
}
