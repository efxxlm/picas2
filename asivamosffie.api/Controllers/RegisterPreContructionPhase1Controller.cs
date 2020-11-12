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
    public class RegisterPreContructionPhase1Controller : Controller
    {
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;


        public RegisterPreContructionPhase1Controller(IRegisterPreContructionPhase1Service registerPreContructionPhase1Service)
        {
            _registerPreContructionPhase1Service = registerPreContructionPhase1Service;
        }




    }
}
