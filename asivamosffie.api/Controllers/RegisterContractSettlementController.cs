using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

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
    }
}