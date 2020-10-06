using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalRequirementsConstructionPhaseController : ControllerBase
    {
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;


        public TechnicalRequirementsConstructionPhaseController(IOptions<AppSettings> settings, IConverter converter, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
            _settings = settings;
            _converter = converter;

        }

        [Route("GetContractsGrid")]
        [HttpGet]
        public async Task<List<dynamic>> GetContractsGrid()
        {
            try
            {
                return await _technicalRequirementsConstructionPhaseService.GetContractsGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

    }
}
