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
        Task<List<VListaContratacionModificacionContractual>> GetListSesionComiteSolicitudV2();

        Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud();

        Task<byte[]> GetDDPBySesionComiteSolicitudID(int pSesionComiteSolicitudID ,string pPatchLogo);

        Task<Respuesta> CambiarEstadoSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
       

        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<Respuesta> RegistrarTramiteContratacion(Contratacion pContratacion, IFormFile pFile, string pDirectorioBase, string pDirectorioMinuta);
    }
}
