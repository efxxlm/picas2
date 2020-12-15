using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IManagePreContructionActPhase1Service
    {

        Task<Respuesta> CreateEditObservacionesActa(ContratoObservacion pcontratoObservacion);

        Task<List<ContratoObservacion>> GetListContratoObservacionByContratoId(int ContratoId );

        Task<dynamic> GetListContrato();

        Task<Contrato> GetContratoByContratoId(int pContratoId, int? pUserId);

        Task<List<GrillaActaInicio>> GetListGrillaActaInicio(int pPerfilId);

        Task<Respuesta> EditContrato(Contrato pContrato);

        Task<Respuesta> LoadActa(Contrato pContrato, IFormFile pFile, string pDirectorioBase, string pDirectorioActaContrato, AppSettingsService appSettingsService);
         
        Task<Respuesta> CambiarEstadoActa(int pContratoId, string pEstadoContrato, string pUsuarioModificacion, AppSettingsService appSettingsService);

        Task<byte[]> GetActaByIdPerfil(int pPerfilId, int pContratoId ,int pUserId, AppSettingsService pAppSettingsService);

        Task GetListContratoConActaSinDocumento(AppSettingsService appSettingsService);
    }
}
