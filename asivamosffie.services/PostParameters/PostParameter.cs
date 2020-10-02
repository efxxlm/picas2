using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.PostParameters
{
    public class PostParameter
    {
        public int solicitudId { get; set; }
        public string aportanteId { get; set; }
        public int plazoMeses { get; set; }
        public int plazoDias { get; set; }
        public string Objeto { get; set; }
    }
}
