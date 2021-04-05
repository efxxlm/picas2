using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IUpdatePoliciesGuaranteesService
    {
        Task<ContratoPoliza> GetContratoPoliza(int pContratoPolizaId);

        Task<Respuesta> DeleteContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro);

        Task<Respuesta> CreateEditContratoPolizaActualizacion(ContratoPolizaActualizacion pContratoPolizaActualizacion);

        Task<List<VActualizacionPolizaYGarantias>> GetListVActualizacionPolizaYGarantias();

        Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato);

        Task<Respuesta> ChangeStatusContratoPolizaActualizacionSeguro(ContratoPolizaActualizacion pContratoPolizaActualizacion);

    }
}
