using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;


namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyPreConstructionRequirementsPhase1Controller : Controller
    {
        public readonly IVerifyPreConstructionRequirementsPhase1Service _verifyPreConstruction;


        public VerifyPreConstructionRequirementsPhase1Controller(IVerifyPreConstructionRequirementsPhase1Service verifyPreConstructionRequirementsPhase1Service)
        {
            _verifyPreConstruction = verifyPreConstructionRequirementsPhase1Service;
        }


        [HttpGet]
        [Route("GetListContratacion")]
        public async Task<List<dynamic>> GetListContratacion()
        {
            return await _verifyPreConstruction.GetListContratacion();
        }


    }
}
