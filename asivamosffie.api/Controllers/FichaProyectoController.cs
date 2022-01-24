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

    public class FichaProyectoController : ControllerBase
    {
        private readonly IFichaProyectoService _fichaProyectoService;


        public FichaProyectoController(IFichaProyectoService fichaProyectoService)
        {
            _fichaProyectoService = fichaProyectoService;
        }
    
        [HttpGet]
        [Route("GetFlujoProyectoByContratacionProyectoId")]
        public async Task<ActionResult<dynamic>> GetFlujoProyectoByContratacionProyectoId(int pContratacionProyectoId)
        {
            return await _fichaProyectoService.GetFlujoProyectoByContratacionProyectoId(pContratacionProyectoId);
        }
        [HttpGet]
        [Route("GetVigencias")]
        public async Task<ActionResult<dynamic>> GetVigencias()
        {
            return await _fichaProyectoService.GetVigencias();
        }

        [HttpGet]
        [Route("GetTablaProyectosByProyectoIdTipoContratacionVigencia")]
        public async Task<ActionResult<dynamic>> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoContrato, string pTipoIntervencion, int pVigencia)
        {
            return await _fichaProyectoService.GetTablaProyectosByProyectoIdTipoContratacionVigencia(pProyectoId, pTipoContrato, pTipoIntervencion, pVigencia);
        }

        [HttpGet]
        [Route("GetProyectoIdByLlaveMen")]
        public async Task<ActionResult<dynamic>> GetProyectoIdByLlaveMen(string pLlaveMen)
        { 
            return await _fichaProyectoService.GetProyectoIdByLlaveMen(pLlaveMen); 
        }
         
 

    }
}
