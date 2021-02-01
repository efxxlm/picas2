using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualModification
    {
        Task<Respuesta> CreateEditarModification(NovedadContractual novedadContractual);

        Task<List<NovedadContractual>> GetListGrillaNovedadContractual();

        Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario);
        Task<ActionResult<List<Contrato>>> GetListContract(int userID);
    }
}
