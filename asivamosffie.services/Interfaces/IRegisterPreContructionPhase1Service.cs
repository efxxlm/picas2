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
    {  //3.1.6
        Task EnviarNotificacion(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        //3.1.7
        Task GetContratosIntrerventoriaSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        //3.1.7
        Task EnviarNotificacionInteventoria(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        //3.1.8
        Task GetContratosObraSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        //3.1.8
        Task GetContratosInterventoriaSinGestionar(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

        Task<List<VRegistrarFase1>> GetListContratacion2();

        Task<dynamic> GetListContratacion();

        Task<Contrato> GetContratoByContratoId(int pContratoId);

        Task<Respuesta> CreateEditContratoPerfil(Contrato pContrato);

        Task<Respuesta> DeleteContratoPerfil(int ContratoPerfilId, string UsuarioModificacion);

        Task<Respuesta> DeleteContratoPerfilNumeroRadicado(int ContratoPerfilNumeroRadicadoId, string UsuarioModificacion);

        Task<Respuesta> ChangeStateContrato(int pContratoId, string UsuarioModificacion, string pEstadoVerificacionContratoCodigo, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

    }
}
