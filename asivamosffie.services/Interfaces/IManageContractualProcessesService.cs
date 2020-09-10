using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IManageContractualProcessesService
    {
        Task<Respuesta> CambiarEstadoSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud)

        Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud();

        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<Respuesta> RegistrarTramiteContratacion(Contratacion pContratacion, IFormFile pFile, string pDirectorioBase, string pDirectorioMinuta);
    }
}
