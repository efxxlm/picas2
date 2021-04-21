using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class TablaDRP
    {
        public int Enum { get; set; }
        public string NumeroDRP { get; set; }
        public string Valor { get; set; }
        public string Saldo { get; set; }
    }
}
