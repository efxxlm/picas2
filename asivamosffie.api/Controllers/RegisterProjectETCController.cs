using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterProjectETCController : Controller
    {

        private readonly IRegisterProjectETCService _registerProjectETCService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterProjectETCController(IRegisterProjectETCService registerProjectETCService, IOptions<AppSettings> settings)
        {
            _registerProjectETCService = registerProjectETCService;
            _settings = settings;
        }
  
        [Route("GetListInformeFinal")]
        [HttpGet]
        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            return await _registerProjectETCService.GetListInformeFinal();
        }


        [Route("GetProyectoEntregaEtc")]
        [HttpGet]
        public async Task<ProyectoEntregaEtc> GetProyectoEntregaEtc([FromQuery] int informeFinalId)
        {
            return await _registerProjectETCService.GetProyectoEntregaEtc(informeFinalId);
        }

        [HttpGet]
        [Route("GetProyectoEntregaETCByInformeFinalId")]
        public async Task<List<dynamic>> GetProyectoEntregaETCByInformeFinalId([FromQuery] int pInformeFinalId)
        {
            try
            {
                return await _registerProjectETCService.GetProyectoEntregaETCByInformeFinalId(pInformeFinalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Creaciones y modificaciones

        [HttpPost]
        [Route("CreateEditRecorridoObra")]

        public async Task<IActionResult> CreateEditRecorridoObra([FromBody] ProyectoEntregaEtc pRecorrido)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pRecorrido.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                //pObservacion.UsuarioCreacion = "LCT";
                respuesta = await _registerProjectETCService.CreateEditRecorridoObra(pRecorrido);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditRepresentanteETC")]

        public async Task<IActionResult> CreateEditRepresentanteETC([FromBody] RepresentanteEtcrecorrido pRepresentante)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pRepresentante.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerProjectETCService.CreateEditRepresentanteETC(pRepresentante);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditRemisionDocumentosTecnicos")]
        public async Task<IActionResult> CreateEditRemisionDocumentosTecnicos([FromBody] ProyectoEntregaEtc pDocumentos)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pDocumentos.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerProjectETCService.CreateEditRemisionDocumentosTecnicos(pDocumentos);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditActaBienesServicios")]
        public async Task<IActionResult> CreateEditActaBienesServicios([FromBody] ProyectoEntregaEtc pActaServicios)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pActaServicios.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerProjectETCService.CreateEditActaBienesServicios(pActaServicios);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("SendProjectToEtc")]
        public async Task<IActionResult> SendProjectToEtc([FromQuery] int informeFinalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerProjectETCService.SendProjectToEtc(informeFinalId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteRepresentanteEtcRecorrido")]
        public async Task<IActionResult> DeleteRepresentanteEtcRecorrido([FromQuery] int representanteEtcId, [FromQuery] int numRepresentantesRecorrido)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerProjectETCService.DeleteRepresentanteEtcRecorrido(representanteEtcId, numRepresentantesRecorrido, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }


    }
}
