using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Route("CreateEditarCuentasBancarias")]
        public async Task<IActionResult> CreateEditarCuentasBancarias(CuentaBancaria cuentaBancaria)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _bankAccount.CreateEditarCuentasBancarias(cuentaBancaria);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
    }
}
