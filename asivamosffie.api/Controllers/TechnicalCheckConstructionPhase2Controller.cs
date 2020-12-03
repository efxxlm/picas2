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
    public class TechnicalCheckConstructionPhase2Controller : Controller
    {
        private readonly ITechnicalCheckConstructionPhase2Service _technicalCheckConstructionPhase2Service;


        public TechnicalCheckConstructionPhase2Controller(ITechnicalCheckConstructionPhase2Service technicalCheckConstructionPhase2Service)
        { 
          _technicalCheckConstructionPhase2Service = technicalCheckConstructionPhase2Service;
        }

        [HttpGet]
        [Route("GetContractsGrid")]
        public Task<List<VRequisitosTecnicosConstruccionAprobar>> GetContractsGrid([FromQuery] string pTipoContrato)
        {
            return _technicalCheckConstructionPhase2Service.GetContractsGrid(" ", pTipoContrato);
        }
    }
}
