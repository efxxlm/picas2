using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IMonitoringURL
    { 
        Task<List<VistaContratoProyectos>> GetListContratoProyectos(); 
        Task<Respuesta> EditarURLMonitoreo(Proyecto pProyecto);
        Task<Respuesta> VisitaURLMonitoreo(string uRLMonitoreo, string usuarioModificacion);
    }
}
