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

    public class RegisterContractSettlement : ControllerBase
    {
        public readonly IRegisterContractSettlementService _registerContractSettlementService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterContractSettlement(IRegisterContractSettlementService pRegisterContractSettlementService, IOptions<AppSettings> settings)
        {
            _registerContractSettlementService = pRegisterContractSettlementService;
            _settings = settings;
        }

        [Route("GetListContractSettlemen")]
        [HttpGet]
        public async Task<dynamic> GetListContractSettlemen()
        {
            return await _registerContractSettlementService.GetListContractSettlemen();
        }
         
    }
}