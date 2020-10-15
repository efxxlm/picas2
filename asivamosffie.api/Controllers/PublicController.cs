﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Responses;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.APIModels;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        public readonly ISourceFundingService _sourceFunding;
        private readonly IOptions<AppSettings> _settings;

        public PublicController(ISourceFundingService sourceFunding, IOptions<AppSettings> settings)
        {
            _sourceFunding = sourceFunding;
            _settings = settings;
        }

        [HttpGet("GetConsignationValue")]
        public async Task GetConsignationValue()
        {
            try
            {
                await _sourceFunding.GetConsignationValue(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                //return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


}
