using asivamosffie.model.APIModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IMonitoringURL
    {

        Task<List<ProyectoGrilla>> GetListProyects();

        Task<List<VistaContratoProyectos>> GetListContratoProyectos();

        Task<Respuesta> EditarURLMonitoreo(Int32 pProyectoId, string URLMonitoreo, string UsuarioModificacion);




    }
}
