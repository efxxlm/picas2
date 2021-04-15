using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FinancialBalanceController : ControllerBase
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IFinalBalanceService _finalBalanceService;
        public FinancialBalanceController(IOptions<AppSettings> settings, IFinalBalanceService finalBalanceService)
        {
            _finalBalanceService = finalBalanceService;
            _settings = settings;
        } 
    }
}
