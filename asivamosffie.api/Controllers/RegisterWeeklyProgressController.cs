using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterWeeklyProgressController : Controller
    {

        private readonly IRegisterWeeklyProgressService _registerWeeklyProgressService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterWeeklyProgressController(IRegisterWeeklyProgressService registerWeeklyProgressService, IOptions<AppSettings> settings)
        {
            _registerWeeklyProgressService = registerWeeklyProgressService;
            _settings = settings;
        }


        [Route("GetVRegistrarAvanceSemanal")]
        [HttpGet]
        public async Task<ActionResult<List<VRegistrarAvanceSemanal>>> GetVRegistrarAvanceSemanal()
        {
            try
            {
                return await _registerWeeklyProgressService.GetVRegistrarAvanceSemanal();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
