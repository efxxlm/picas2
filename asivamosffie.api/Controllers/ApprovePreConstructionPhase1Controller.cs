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
        public readonly IApprovePreConstructionPhase1Service _approvePreConstructionPhase1Service;

        public ApprovePreConstructionPhase1Controller(IApprovePreConstructionPhase1Service approvePreConstructionPhase1Service)
        {
            _approvePreConstructionPhase1Service = approvePreConstructionPhase1Service;
        } 
 


    }
}
