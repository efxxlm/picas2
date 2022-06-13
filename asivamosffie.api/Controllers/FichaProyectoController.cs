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
        [Route("GetInfoSeguimientoEntregaETCByProyectoId")]
        public async Task<ActionResult<dynamic>> GetInfoSeguimientoEntregaETCByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetInfoSeguimientoEntregaETCByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetInfoSeguimientoTecnicoByProyectoId")]
        public async Task<ActionResult<dynamic>> GetInfoSeguimientoTecnicoByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetInfoSeguimientoTecnicoByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetInfoContratoByProyectoId")]
        public async Task<ActionResult<dynamic>> GetInfoContratoByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetInfoContratoByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetInfoResumenByProyectoId")]
        public async Task<ActionResult<dynamic>> GetInfoResumenByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetInfoResumenByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetInfoPreparacionByProyectoId")]
        public async Task<ActionResult<dynamic>> GetInfoPreparacionByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetInfoPreparacionByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetFlujoProyectoByProyectoId")]
        public async Task<ActionResult<dynamic>> GetFlujoProyectoByProyectoId(int pProyectoId)
        {
            return await _fichaProyectoService.GetFlujoProyectoByProyectoId(pProyectoId);
        }
        [HttpGet]
        [Route("GetVigencias")]
        public async Task<ActionResult<dynamic>> GetVigencias()
        {
            return await _fichaProyectoService.GetVigencias();
        }
        [HttpGet]
        [Route("GetTablaProyectosByProyectoIdTipoContratacionVigencia")]
        public async Task<ActionResult<dynamic>> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoIntervencion, int pVigencia)
        {
            return await _fichaProyectoService.GetTablaProyectosByProyectoIdTipoContratacionVigencia(pProyectoId, pTipoIntervencion, pVigencia);
        }
        [HttpGet]
        [Route("GetProyectoIdByLlaveMen")]
        public async Task<ActionResult<dynamic>> GetProyectoIdByLlaveMen(string pLlaveMen)
        {
            return await _fichaProyectoService.GetProyectoIdByLlaveMen(pLlaveMen);
        }
    }
}
