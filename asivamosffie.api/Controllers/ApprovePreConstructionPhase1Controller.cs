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

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ApprovePreConstructionPhase1Controller : Controller
    {
        public readonly IApprovePreConstructionPhase1Service _approvePreConstruction;

        public ApprovePreConstructionPhase1Controller(IApprovePreConstructionPhase1Service approvePreConstructionPhase1Service)
        {
            _approvePreConstruction = approvePreConstructionPhase1Service;
        }

        [Route("GetListContratacion")]
        [HttpGet]
        public async Task<List<dynamic>> GetListContratacion()
        {
            var result = await _approvePreConstruction.GetListContratacion();
            return result;
        }

    }
}
