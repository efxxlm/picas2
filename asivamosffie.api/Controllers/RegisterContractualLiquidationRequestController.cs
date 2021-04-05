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

        [Route("GridRegisterContractualLiquidationObra")]
        [HttpGet]
        public async Task<ActionResult<List<VContratacionProyectoSolicitudLiquidacion>>> GridRegisterContractualLiquidationObra()
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridRegisterContractualLiquidationObra();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GridRegisterContractualLiquidationInterventoria")]
        [HttpGet]
        public async Task<ActionResult<List<VContratacionProyectoSolicitudLiquidacion>>> GridRegisterContractualLiquidationInterventoria()
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridRegisterContractualLiquidationInterventoria();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GridInformeFinal")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GridInformeFinal(int pContratacionProyectoId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GridInformeFinal(pContratacionProyectoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetInformeFinalByProyectoId")]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetInformeFinalByProyectoId(int pProyectoId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetInformeFinalByProyectoId(pProyectoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetInformeFinalAnexoByInformeFinalId")]
        [HttpGet]
        public async Task<ActionResult<List<InformeFinalInterventoria>>> GetInformeFinalAnexoByInformeFinalId(int pInformeFinalId)
        {
            try
            {
                return await _registerContractualLiquidationRequest.GetInformeFinalAnexoByInformeFinalId(pInformeFinalId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}