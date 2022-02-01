using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionesLineaPagosController : ControllerBase
    {
        public readonly IValidacionesLineaPagoServices _ValidacionesLineaPagoServices;
        private readonly IOptions<AppSettings> _settings;

        public ValidacionesLineaPagosController(IValidacionesLineaPagoServices validacionesLineaPagoServices, IOptions<AppSettings> settings)
        {
            _ValidacionesLineaPagoServices = validacionesLineaPagoServices;
            _settings = settings;
        }

        [HttpGet]
        [Route("ValidacionFacturadosODG")]
        public async Task<ActionResult<dynamic>> ValidacionFacturadosODG()
        { 
           return await _ValidacionesLineaPagoServices.ValidacionFacturadosODG();
        }
            
 

    }
}