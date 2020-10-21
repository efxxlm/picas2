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
    public class ActBeginController : ControllerBase
    {
        public readonly IActBeginService _ActBegin;
        private readonly IOptions<AppSettings> _settings;

        public ActBeginController(IActBeginService actBegin, IOptions<AppSettings> settings)
        {
            _ActBegin = actBegin;
            _settings = settings;

        }

        //public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetListVistaGenerarActaInicio(int pContratoId)
        [Route("GetVistaGenerarActaInicio")]
        [HttpGet]        
        public async Task<ActionResult<VistaGenerarActaInicioContrato>> GetVistaGenerarActaInicio(int pContratoId)
        {
            try
            {
                return await _ActBegin.GetListVistaGenerarActaInicio(pContratoId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }        

        [HttpGet]
        [Route("GetListGrillaActaInicio")]
        public async Task<ActionResult<List<GrillaActaInicio>>> GetListGrillaActaInicio()
        {
            try
            {
                return await _ActBegin.GetListGrillaActaInicio();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("GetPlantillaActaInicio")]
        public async Task<FileResult> GetPlantillaActaInicio(int pContratoId)
        {
            return File(await _ActBegin.GetPlantillaActaInicio(pContratoId), "application/pdf");
        }


        [HttpPost]
        [Route("EditCargarActaSuscritaContrato")]
       
        public async Task<IActionResult> EditCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria
             , IFormFile pFile,  string pUsuarioModificacion
          )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.GuardarCargarActaSuscritaContrato( pContratoId,  pFechaFirmaContratista,  pFechaFirmaActaContratistaInterventoria                     
             ,  pFile, _settings.Value.DirectoryBase, _settings.Value.DirectoryBaseActaInicio,  pUsuarioModificacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreatePlazoEjecucionFase2Construccion")]
        
        public async Task<IActionResult> CreatePlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2    )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.GuardarPlazoEjecucionFase2Construccion( pContratoId,  pPlazoFase2PreMeses,  pPlazoFase2PreDias,  pObservacionesConsideracionesEspeciales,  pUsuarioModificacion,  pFechaActaInicioFase1,  pFechaTerminacionFase2);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        

        [HttpPost]
        [Route("CreateTieneObservacionesActaInicio")]

        public async Task<IActionResult> CreateTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion
          )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.GuardarTieneObservacionesActaInicio( pContratoId,  pObservacionesActa,  pUsuarioModificacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("EditarContratoObservacion")]        
        public async Task<IActionResult> EditarContratoObservacion(int pContratoId, string pObservacion, string pUsuarioModificacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _ActBegin.EditarContratoObservacion( pContratoId,  pObservacion,  pUsuarioModificacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


    }

}
