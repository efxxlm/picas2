using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

 
    namespace asivamosffie.model.Models
    {
        public partial class Programacion
        {
            [NotMapped] 
            public double RangoDias { get; set; }
        }
 