using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Version
    {
        public int VersionId { get; set; }
        public int Back { get; set; }
        public int Front { get; set; }
        public DateTime FechaDespliegue { get; set; }
    }
}
