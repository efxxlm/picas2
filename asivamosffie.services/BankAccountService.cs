using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public BankAccountService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
        }

        public async Task<List<CuentaBancaria>> GetBankAccount()
        {
            return await _context.CuentaBancaria.ToListAsync();
        }

        public async Task<CuentaBancaria> GetBankAccountById(int id)
        {
            return await _context.CuentaBancaria.FindAsync(id);
        }
        public async Task<Respuesta> Insert(CuentaBancaria cuentaBancaria)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                if (cuentaBancaria != null)
                {
                    _context.Add(cuentaBancaria);
                    await _context.SaveChangesAsync();
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Data = cuentaBancaria, Code = ConstantMessagesContributor.OperacionExitosa };
                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContributor.CamposIncompletos };
                }

            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno, Message = ex.InnerException.ToString() };
            }

            return respuesta;
        }

        public Task<bool> Update(CuentaBancaria cuentaBancaria)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                CuentaBancaria entity = await GetBankAccountById(id);
                _context.CuentaBancaria.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}