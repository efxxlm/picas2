using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterValidatePaymentRequierementsService
    {
        Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato);

        Task<Contrato> GetContratoByContratoId(int pContratoId);

        Task<dynamic> GetProyectosByIdContrato(int pContratoId);
    }
}
