using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BankAccountController : ControllerBase
    {
        public readonly IBankAccountService _bankAccount;


        public BankAccountController(IBankAccountService bankAccount)
        {
            _bankAccount = bankAccount;
        }

        [HttpGet]
        public async Task<List<CuentaBancaria>> Get()
        {
            try
            {
                var result = await _bankAccount.GetBankAccount();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _bankAccount.GetBankAccountById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> post(CuentaBancaria cuentabancaria)
        {
            try
            {
                var result = await _bankAccount.Insert(cuentabancaria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
