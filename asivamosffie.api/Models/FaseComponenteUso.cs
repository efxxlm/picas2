using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class FaseComponenteUso
    {
        public long FaseComponenteUsoId { get; set; }
        public string FaseId { get; set; }
        public string ComponenteId { get; set; }
        public string UsoId { get; set; }
    }
}
