using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JudicialDefenseController : ControllerBase
    {

        public readonly IJudicialDefense _judicialDefense;

        public JudicialDefenseController(IJudicialDefense judicialDefense)
        {
            _judicialDefense = judicialDefense;
        }


        [HttpPost]
        [Route("CreateOrEditDemandadoConvocado")]        
        public async Task<IActionResult> CreateOrEditDemandadoConvocado(DemandadoConvocado demandadoConvocado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (demandadoConvocado.DemandadoConvocadoId  == 0)
                    demandadoConvocado.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    demandadoConvocado.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;

                respuesta = await _judicialDefense.CreateOrEditDemandadoConvocado(demandadoConvocado);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        [HttpGet]
        [Route("GetPlantillaDefensaJudicial")]
        public async Task<FileResult> GetPlantillaDefensaJudicial(int pContratoId)
        {
            return File(await _judicialDefense.GetPlantillaDefensaJudicial(pContratoId), "application/pdf");
        }


        [HttpPost]
        [Route("CreateOrEditDefensaJudicial")]
        public async Task<IActionResult> CreateOrEditDefensaJudicial(DefensaJudicial defensaJudicial)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (defensaJudicial.DefensaJudicialId == 0)
                    defensaJudicial.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    defensaJudicial.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;

                respuesta = await _judicialDefense.CreateOrEditDefensaJudicial(defensaJudicial);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetNombreContratistaByContratoId")]
        [HttpGet]
        
        public async Task<string> GetNombreContratistaByContratoId(int pContratoId)
        {
            var respuesta = await _judicialDefense.GetNombreContratistaByContratoId(pContratoId);
            return respuesta;
        }        

        [HttpPost]
        [Route("CreateOrEditFichaEstudio")]
        public async Task<IActionResult> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (fichaEstudio.FichaEstudioId == 0)
                    fichaEstudio.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    fichaEstudio.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;

                respuesta = await _judicialDefense.CreateOrEditFichaEstudio(fichaEstudio);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
                
        [HttpGet]
        [Route("GetListProyects")]
        public async Task<ActionResult<List<ProyectoGrilla>>> GetListProyects(int pProyectoId)
        {
            try
            {
                return await _judicialDefense.GetListProyects(pProyectoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        [HttpGet]
        [Route("GetListGrillaProcesosDefensaJudicial")]
        public async Task<ActionResult<List<GrillaProcesoDefensaJudicial>>> ListGrillaProcesosDefensaJudicial()
        {
            try
            {
                return await _judicialDefense.ListGrillaProcesosDefensaJudicial();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
