using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePreContructionActPhase1Controller : Controller
    {
        private readonly IManagePreContructionActPhase1Service _managePreContruction;
        public ManagePreContructionActPhase1Controller(IManagePreContructionActPhase1Service managePreContructionActPhase1Service) {
            _managePreContruction = managePreContructionActPhase1Service;
        }
         
        [Route("GetListContrato")]
        [HttpGet]
        public async Task <List<dynamic>> GetListContrato()
        {
            return await _managePreContruction.GetListContrato();
        }

        [Route("GetContratoByContratoId")]
        [HttpGet]
        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            return await _managePreContruction.GetContratoByContratoId(pContratoId);
        }

        
    }
}
