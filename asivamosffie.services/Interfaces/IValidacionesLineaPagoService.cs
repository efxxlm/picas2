using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
 
namespace asivamosffie.services.Interfaces
{
    public interface IValidacionesLineaPagoServices 
    {
         Task<List<VOdgValoresFacturados>> ValidacionFacturadosODG();
    }
}
