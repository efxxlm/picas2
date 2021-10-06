using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReleaseBalanceController : Controller
    {

        private readonly IReleaseBalanceService _releaseBalanceService;
        private readonly IOptions<AppSettings> _settings;

        public ReleaseBalanceController(IReleaseBalanceService releaseBalanceService, IOptions<AppSettings> settings)
        {
            _releaseBalanceService = releaseBalanceService;
            _settings = settings;
        }

        [Route("GetDrpByProyectoId")]
        [HttpGet]
        public async Task<List<dynamic>> GetDrpByProyectoId([FromQuery] int pProyectoId)
        {
            return await _releaseBalanceService.GetDrpByProyectoId(pProyectoId);
        }

    }
}
