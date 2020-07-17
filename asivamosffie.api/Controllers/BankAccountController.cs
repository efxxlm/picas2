using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
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
        public async Task<ActionResult<List<CuentaBancaria>>> Get()
        {
            try
            {
                return await _bankAccount.GetBankAccount();
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


        [HttpPut]
        public async Task<IActionResult> update(CuentaBancaria cuentabancaria)
        {
            Respuesta _response = new Respuesta();

            try
            {
                _response = await _bankAccount.Update(cuentabancaria);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }


    }
}
