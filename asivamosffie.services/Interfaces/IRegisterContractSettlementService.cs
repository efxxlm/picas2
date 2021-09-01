using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterContractSettlementService
    {
        Task<List<dynamic>> GetListContractSettlemen();

        Task<Respuesta> CreateEditContractSettlement(Contratacion pContratacion);

        Task<Respuesta> ChangeStateContractSettlement(int pContratacionId, string user);
    }
}
