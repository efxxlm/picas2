﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractualNoveltyController : ControllerBase
    {
        public readonly IContractualNoveltyService _contractualModification;

        public ContractualNoveltyController(IContractualNoveltyService contractualModification)
        {
            _contractualModification = contractualModification;
        }

        /*autor: jflorez
           descripción: trae listado de contratos asignados al usuario logeado para el autocompletar
           impacto: CU 4.1.3*/
        [HttpGet]
        [Route("GetListContract")]
        public async Task<ActionResult<List<Contrato>>> GetListContract()
        {
            try
            {
                int pUserId = Int32.Parse(HttpContext.User.FindFirst("UserId").Value);
                return await _contractualModification.GetListContract(pUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*autor: jflorez
           descripción: guarda
           impacto: CU 4.1.3*/
        [HttpPost]
        [Route("CreateEditNovedadContractual")]
        public async Task<IActionResult> CreateEditNovedadContractual(NovedadContractual novedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                novedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualModification.CreateEditNovedadContractual(novedadContractual);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditNovedadContractualTramite")]
        public async Task<IActionResult> CreateEditNovedadContractualTramite(NovedadContractual novedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                novedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualModification.CreateEditNovedadContractualTramite(novedadContractual);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*autor: jflorez
           descripción: grilla de novedades
           impacto: CU 4.1.3*/
        [HttpGet]
        [Route("GetListGrillaNovedadContractualObra")]
        public async Task<ActionResult<List<VNovedadContractual>>> GetListGrillaNovedadContractualObra()
        {
            try
            {
                return await _contractualModification.GetListGrillaNovedadContractualObra();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaNovedadContractualInterventoria")]
        public async Task<ActionResult<List<VNovedadContractual>>> GetListGrillaNovedadContractualInterventoria()
        {
            try
            {
                return await _contractualModification.GetListGrillaNovedadContractualInterventoria();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaNovedadContractualGestionar")]
        public async Task<ActionResult<List<VNovedadContractual>>> GetListGrillaNovedadContractualGestionar()
        {
            try
            {
                return await _contractualModification.GetListGrillaNovedadContractualGestionar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*autor: jflorez
           descripción: elimina
           impacto: CU 4.1.3*/
        [HttpDelete]
        [Route("EliminarNovedadContractual")]
        public async Task<IActionResult> EliminarNovedadContractual([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarNovedadContractual(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     

        [HttpDelete]
        [Route("EliminarNovedadContractualAportante")]
        public async Task<IActionResult> EliminarNovedadContractualAportante([FromQuery] int pNovedadContractualAportante)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarNovedadContractualAportante(pNovedadContractualAportante, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("EliminarComponenteAportanteNovedad")]
        public async Task<IActionResult> EliminarComponenteAportanteNovedad([FromQuery] int pComponenteAportanteNovedad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarComponenteAportanteNovedad(pComponenteAportanteNovedad, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpDelete]
        [Route("EliminarComponenteFuenteNovedad")]
        public async Task<IActionResult> EliminarComponenteFuenteNovedad([FromQuery] int pComponenteFuenteNovedad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarComponenteFuenteNovedad(pComponenteFuenteNovedad, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("EliminarComponenteUsoNovedad")]
        public async Task<IActionResult> EliminarComponenteUsoNovedad([FromQuery] int pComponenteUsoNovedad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarComponenteUsoNovedad(pComponenteUsoNovedad, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("EliminarNovedadClausula")]
        public async Task<IActionResult> EliminarNovedadClausula([FromQuery] int pNovedadContractuaClausulalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EliminarNovedadClausula(pNovedadContractuaClausulalId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("AprobarSolicitud")]
        public async Task<IActionResult> AprobarSolicitud([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.AprobarSolicitud(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("EnviarAlSupervisor")]
        public async Task<IActionResult> EnviarAlSupervisor([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EnviarAlSupervisor(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("TramitarSolicitud")]
        public async Task<IActionResult> TramitarSolicitud([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.TramitarSolicitud(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("RechazarPorInterventor")]
        public async Task<IActionResult> RechazarPorInterventor([FromBody] NovedadContractual pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.RechazarPorInterventor(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("RechazarPorSupervisor")]
        public async Task<IActionResult> RechazarPorSupervisor([FromBody] NovedadContractual pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.RechazarPorSupervisor(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("DevolverSolicitud")]
        public async Task<IActionResult> DevolverSolicitud([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.DevolverSolicitud(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("DevolverSolicitudASupervisor")]
        public async Task<IActionResult> DevolverSolicitudASupervisor([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.DevolverSolicitudASupervisor(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("EnviarAComite")]
        public async Task<IActionResult> EnviarAComite([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.EnviarAComite(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetProyectsByContract")]
        public async Task<ActionResult<List<VProyectosXcontrato>>> GetProyectsByContract(int pContratoId)
        {
            try
            {
                return await _contractualModification.GetProyectsByContract(pContratoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFuentesByAportante")]
        public async Task<List<FuenteFinanciacion>> GetFuentesByAportante([FromQuery]int pConfinanciacioAportanteId)
        {
            try
            {
                return await _contractualModification.GetFuentesByAportante(pConfinanciacioAportanteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetNovedadContractualById")]
        public async Task<NovedadContractual> GetNovedadContractualById([FromQuery] int pId)
        {
            try
            {
                return await _contractualModification.GetNovedadContractualById(pId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditObservacion")]
        [HttpPost]
        public async Task<Respuesta> CreateEditObservacion([FromBody] NovedadContractual pNovedadContractual, [FromQuery] bool? esSupervisor, bool? esTramite)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pNovedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualModification.CreateEditObservacion(pNovedadContractual, esSupervisor, esTramite);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAportanteByContratacion")]
        public async Task<List<CofinanciacionAportante>> GetAportanteByContratacion([FromQuery] int pId)
        {
            try
            {
                return await _contractualModification.GetAportanteByContratacion(pId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("CancelarNovedad")]
        public async Task<IActionResult> CancelarNovedad([FromQuery] int pNovedadContractualId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.CancelarNovedad(pNovedadContractualId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("AprobacionTecnicaJuridica")]
        public async Task<IActionResult> AprobacionTecnicaJuridica([FromQuery] int pNovedaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualModification.AprobacionTecnicaJuridica(pNovedaContractual, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
