using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualNoveltyService
    {
        Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual);
        Task<List<NovedadContractual>> GetListGrillaNovedadContractual();
        Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario);
        Task<List<Contrato>> GetListContract(int userID);
        Task<List<VProyectosXcontrato>> GetProyectsByContract(int pContratoId);
    }
}
