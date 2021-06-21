using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiro
    {
        [NotMapped]
        public int SolicitudPagoId { get; set; }

        [NotMapped]
        public dynamic TablaFacturado { get; set; }

        [NotMapped]
        public dynamic TablaDescuento { get; set; }

        [NotMapped]
        public dynamic TablaOtroDescuento { get; set; }

    }
}
