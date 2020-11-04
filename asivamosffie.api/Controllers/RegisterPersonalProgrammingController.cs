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
    public class RegisterPersonalProgrammingController : Controller
    {
        public readonly IRegisterPersonalProgrammingService _IRegisterPersonalProgrammingService;


        public RegisterPersonalProgrammingController(IRegisterPersonalProgrammingService registerPersonalProgrammingService)
        {

           _IRegisterPersonalProgrammingService = registerPersonalProgrammingService;
        }
        [Route("GetListProyectos")]
        [HttpGet]
        public async Task<List<dynamic>> GetListProyectos()
        {
            var result = await _IRegisterPersonalProgrammingService.GetListProyectos();
            return result;
        }


    }
}
