using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class IndicadorReporte
    {
        [NotMapped]
        public EmbedParams EmbedParams { get; set; }
    }


}
