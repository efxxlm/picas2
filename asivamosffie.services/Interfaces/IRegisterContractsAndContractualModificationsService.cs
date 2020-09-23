using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterContractsAndContractualModificationsService
    {
        Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud();

        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<Respuesta> RegistrarTramiteContrato(Contrato pContrato ,string pPatchfile,string pEstadoCodigo);
    }
}
