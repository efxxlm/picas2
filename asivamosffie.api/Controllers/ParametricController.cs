﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ParametricController : Controller
    { 
        private readonly IParametricService _parametricService;
        private readonly IOptions<AppSettings> _settings;

        public ParametricController(IParametricService parametricService, IOptions<AppSettings> settings)
        {
            _parametricService = parametricService;
            _settings = settings;
        }

       
        [HttpGet]
        [Route("GetParametricas")]
        public async Task<List<VParametricas>> GetParametricas()
        {
            return await _parametricService.GetParametricas();
        }



    }
}