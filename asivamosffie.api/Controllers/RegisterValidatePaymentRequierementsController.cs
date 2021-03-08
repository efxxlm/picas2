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
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterValidateSpinOrderController : Controller
    {

        private readonly IOptions<AppSettings> _settings;
        private readonly IRegisterValidateSpinOrderService _registerValidateSpinOrderService;

        public RegisterValidateSpinOrderController(IOptions<AppSettings> settings, IRegisterValidateSpinOrderService registerValidateSpinOrderService)
        {
            _registerValidateSpinOrderService = registerValidateSpinOrderService;
            _settings = settings;
        }

        [Route("CreateEditSpinOrderObservations")]
        [HttpPost]
        public async Task<IActionResult> CreateEditSpinOrderObservations([FromBody] OrdenGiroObservacion pOrdenGiroObservacion)
        {
            try
            {
                var result = await _registerValidateSpinOrderService.CreateEditSpinOrderObservations(pOrdenGiroObservacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
