using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace asivamosffie.services.Interfaces
{
    public interface IRegisterPreContructionPhase1Service
    {
        Task EnviarNotificacion(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

        Task<List<VRegistrarFase1>> GetListContratacion2();

        Task<dynamic> GetListContratacion();

        Task<Contrato> GetContratoByContratoId(int pContratoId);

        Task<Respuesta> CreateEditContratoPerfil(Contrato pContrato);

        Task<Respuesta> DeleteContratoPerfil(int ContratoPerfilId, string UsuarioModificacion);

        Task<Respuesta> DeleteContratoPerfilNumeroRadicado(int ContratoPerfilNumeroRadicadoId, string UsuarioModificacion);

        Task<Respuesta> ChangeStateContrato(int pContratoId, string UsuarioModificacion, string pEstadoVerificacionContratoCodigo, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

    }
}
