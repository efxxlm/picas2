using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace asivamosffie.api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class FichaContratoController : ControllerBase
    {
        private readonly IFichaContratoService _fichaContratoService;


        public FichaContratoController(IFichaContratoService fichaContratoService)
        {
            _fichaContratoService = fichaContratoService;
        }
 
        [HttpGet]
        [Route("GetInfoNovedadesByContratoId")]
        public async Task<ActionResult<dynamic>> GetInfoNovedadesByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetInfoNovedadesByContratoId(pContratoId);
        }  
          [HttpGet]
        [Route("GetInfoPolizasSegurosByContratoId")]
        public async Task<ActionResult<dynamic>> GetInfoPolizasSegurosByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetInfoPolizasSegurosByContratoId(pContratoId);
        }  
        

        [HttpGet]
        [Route("GetInfoContratacionByContratoId")]
        public async Task<ActionResult<dynamic>> GetInfoContratacionByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetInfoContratacionByContratoId(pContratoId);
        }   
           
        [HttpGet]
        [Route("GetInfoProcesosSeleccionByContratoId")]
        public async Task<ActionResult<dynamic>> GetInfoProcesosSeleccionByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetInfoProcesosSeleccionByContratoId(pContratoId);
        }   
        
        
        [HttpGet]
        [Route("GetInfoResumenByContratoId")]
        public async Task<ActionResult<dynamic>> GetInfoResumenByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetInfoResumenByContratoId(pContratoId);
        }


        [HttpGet]
        [Route("GetFlujoContratoByContratoId")]
        public async Task<ActionResult<dynamic>> GetFlujoContratoByContratoId(int pContratoId)
        {
            return await _fichaContratoService.GetFlujoContratoByContratoId(pContratoId);
        }

        [HttpGet]
        [Route("GetContratosByNumeroContrato")]
        public async Task<ActionResult<dynamic>> GetContratosByNumeroContrato(string pNumeroContrato)
        {
            return await _fichaContratoService.GetContratosByNumeroContrato(pNumeroContrato);
        }
    }
}
