using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterContractSettlementController : ControllerBase
    {
        public readonly IRegisterContractSettlementService _registerContractSettlementService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterContractSettlementController(IRegisterContractSettlementService pRegisterContractSettlementService, IOptions<AppSettings> settings)
        {
            _registerContractSettlementService = pRegisterContractSettlementService;
            _settings = settings;
        }

        [Route("GetListContractSettlemen")]
        [HttpGet]
        public async Task<List<dynamic>> GetListContractSettlemen()
        {
            return await _registerContractSettlementService.GetListContractSettlemen();
        }
        [Route("CreateEditContractSettlement")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContractSettlement([FromBody] Contratacion pContratacion)
        {
            try
            {
                pContratacion.UsuarioModificacion = User.Identity.Name;
                return Ok(await _registerContractSettlementService.CreateEditContractSettlement(pContratacion));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("ChangeStateContractSettlement")]
        [HttpPost]
        public async Task<IActionResult> ChangeStateContractSettlement([FromQuery] int pContratacionId)
        {
            try
            {
                return Ok(await _registerContractSettlementService.ChangeStateContractSettlement(pContratacionId, User.Identity.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}