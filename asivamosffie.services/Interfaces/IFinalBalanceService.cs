using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IFinalBalanceService
    {
        Task<List<VProyectosBalance>> GridBalance();
        Task<List<dynamic>> GetContratoByProyectoId(int pProyectoId);
        Task<Respuesta> CreateEditBalanceFinanciero(BalanceFinanciero pBalanceFinanciero);
        Task<BalanceFinanciero> GetBalanceFinanciero(int pProyectoId);
        Task<Respuesta> ApproveBalance(int pProyectoId, string pUsuario);
        Task<dynamic> GetDataByProyectoId(int pProyectoId);
    }
}
