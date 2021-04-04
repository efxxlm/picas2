using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using asivamosffie.api.Responses;
using System.Security.Claims;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePoliciesGuaranteesController : ControllerBase
    {
        public readonly IUpdatePoliciesGuaranteesService _updatePoliciesGuaranteesService;
    

        public UpdatePoliciesGuaranteesController(IUpdatePoliciesGuaranteesService updatePoliciesGuaranteesService)
        {
            _updatePoliciesGuaranteesService = updatePoliciesGuaranteesService;
        }

        [Route("GetContratoByNumeroContrato")]
        [HttpGet]
        public async Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato)
        {
            var respuesta = await _updatePoliciesGuaranteesService.GetContratoByNumeroContrato(pNumeroContrato);
            return respuesta;
        }
         
    }
}
