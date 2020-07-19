using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IBankAccountService
    {
        Task<ActionResult<List<CuentaBancaria>>> GetBankAccount();
        Task<CuentaBancaria> GetBankAccountById(int id);

        Task<Respuesta> Insert(CuentaBancaria cuentaBancaria);

        Task<Respuesta> Update(CuentaBancaria cuentaBancaria);

        Task<bool> Delete(int id);

        Task<List<FuenteFinanciacion>> GetFuentesFinanciacionByAportanteId(int AportanteId);

        Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion();
    }
}
