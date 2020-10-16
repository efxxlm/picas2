using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ComiteTecnico
    {
        [NotMapped]
        public int NumeroCompromisos { get; set; }
        [NotMapped]
        public int NumeroCompromisosCumplidos { get; set; }
        
    }

}
