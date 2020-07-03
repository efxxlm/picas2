using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IBankAccountService
    {
        Task<List<CuentaBancaria>> GetBankAccount();
        Task<CuentaBancaria> GetBankAccountById(int id);

        Task<Respuesta> Insert(CuentaBancaria cuentaBancaria);

        Task<bool> Update(CuentaBancaria cuentaBancaria);

        Task<bool> Delete(int id);
    }
}
