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
    [Authorize]
    public class RegisterContractualLiquidationRequestController : ControllerBase
    {
        public readonly IRegisterContractualLiquidationRequestService _registerContractualLiquidationRequest;
        private readonly IOptions<AppSettings> _settings;

        public RegisterContractualLiquidationRequestController(IRegisterContractualLiquidationRequestService pRegisterContractualLiquidationRequestService, IOptions<AppSettings> settings)
        {
            _registerContractualLiquidationRequest = pRegisterContractualLiquidationRequestService;
            _settings = settings;
        }

        [Route("gridRegisterContractualLiquidationRequest")]
        [HttpGet]
        public async Task<ActionResult<List<VContratacionProyectoSolicitudLiquidacion>>> gridRegisterContractualLiquidationRequest()
        {
            try
            {
                return await _registerContractualLiquidationRequest.gridRegisterContractualLiquidationRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}