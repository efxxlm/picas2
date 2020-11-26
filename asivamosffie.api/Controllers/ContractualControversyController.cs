using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractualControversyController : ControllerBase
    {
        public readonly IContractualControversy _contractualControversy;

        public ContractualControversyController(IContractualControversy contractualControversy)
        {
            _contractualControversy = contractualControversy;
        }

        [HttpPost]
        [Route("CreateEditarControversiaTAI")]
        public async Task<IActionResult> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (controversiaContractual.ControversiaContractualId == 0)
                    controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    controversiaContractual.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditarControversiaTAI(controversiaContractual);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditarControversiaMotivo")]
        public async Task<IActionResult> CreateEditarControversiaMotivo(ControversiaMotivo controversiaMotivo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (controversiaMotivo.ControversiaMotivoId == 0)
                    controversiaMotivo.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    controversiaMotivo.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.InsertEditControversiaMotivo(controversiaMotivo);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditarActuacionSeguimiento")]
        public async Task<IActionResult> CreateEditarActuacionSeguimiento(ActuacionSeguimiento actuacionSeguimiento)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (actuacionSeguimiento.ActuacionSeguimientoId == 0)
                    actuacionSeguimiento.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    actuacionSeguimiento.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditarActuacionSeguimiento(actuacionSeguimiento);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        //[Route("CreateEditNuevaActualizacionTramite")]
        //public async Task<IActionResult> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)

        [HttpPost]        
        [Route("CreateEditControversiaOtros")]
        public async Task<IActionResult> CreateEditControversiaOtros(ControversiaActuacion controversiaActuacion)
        
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //controversiaContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (controversiaActuacion.ControversiaActuacionId == 0)
                    controversiaActuacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                    controversiaActuacion.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualControversy.CreateEditControversiaOtros(controversiaActuacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        
        [HttpGet]
        [Route("GetListGrillaActuacionSeguimiento")]
        public async Task<ActionResult<List<GrillaActuacionSeguimiento>>> GetListGrillaActuacionSeguimiento()
        {
            try
            {
                return await _contractualControversy.ListGrillaActuacionSeguimiento();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaTipoSolicitudControversiaContractual")]
        public async Task<ActionResult<List<GrillaTipoSolicitudControversiaContractual>>> GetListGrillaTipoSolicitudControversiaContractual()
        {
            try
            {
                return await _contractualControversy.ListGrillaTipoSolicitudControversiaContractual();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListGrillaControversiaActuacion")]

        public async Task<ActionResult<List<GrillaControversiaActuacionEstado>>> GetListGrillaControversiaActuacion(int id = 0)
        {
            try
            {
                return await _contractualControversy.ListGrillaControversiaActuacion(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetVistaContratoContratista")]
        public async Task<ActionResult<VistaContratoContratista>> GetVistaContratoContratista(int pContratoId)
        {
            try
            {
                return await _contractualControversy.GetVistaContratoContratista(pContratoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("CambiarEstadoControversiaContractual")]
        public async Task<IActionResult> CambiarEstadoControversiaContractual([FromQuery] int pControversiaContractualId, string pNuevoCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.CambiarEstadoControversiaContractual(pControversiaContractualId, pNuevoCodigoEstado, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }





        [Route("EliminarControversiaContractual")]
        [HttpPost]
        public async Task<IActionResult> EliminarControversiaContractual(int pControversiaContractualId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _contractualControversy.EliminarControversiaContractual(pControversiaContractualId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("GetControversiaContractualById")]
        [HttpGet]

        public async Task<ControversiaContractual> GetControversiaContractualById(int pControversiaContractualId)
        {
            var respuesta = await _contractualControversy.GetControversiaContractualById(pControversiaContractualId);
            return respuesta;
        }
                
        
        [Route("GetMotivosSolicitudByControversiaId")]
        [HttpGet]
        public async Task<List<ControversiaMotivo>> GetMotivosSolicitudByControversiaId(int id)
        {
            var respuesta = await _contractualControversy.GetMotivosSolicitudByControversiaContractualId(id);
            return respuesta;
        }
        [Route("GetControversiaActuacionById")]
        [HttpGet]
        public async Task<ControversiaActuacion> GetControversiaActuacionById(int pControversiaActuacionId)
        {
            var respuesta = await _contractualControversy.GetControversiaActuacionById(pControversiaActuacionId);
            return respuesta;
        }

        [Route("GetListContratos")]
        [HttpGet]
        public async Task<List<Contrato>> GetListContratos()
        {
            var respuesta = await _contractualControversy.GetListContratos();
            return respuesta;
        }


        [Route("EliminarControversiaActuacion")]
        [HttpPost]
        public async Task<IActionResult> EliminarControversiaActuacion(int pControversiaActuacionId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _contractualControversy.EliminarControversiaActuacion(pControversiaActuacionId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



        [HttpPost]
        [Route("ActualizarRutaSoporteControversiaContractual")]
        public async Task<IActionResult> ActualizarRutaSoporteControversiaContractual( int pControversiaContractualId, string pRutaSoporte)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.ActualizarRutaSoporteControversiaContractual(pControversiaContractualId, pRutaSoporte, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        

        [HttpPost]
        [Route("ActualizarRutaSoporteControversiaActuacion")]
        public async Task<IActionResult> ActualizarRutaSoporteControversiaActuacion([FromQuery] int pControversiaActuacionId, string pRutaSoporte)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.ActualizarRutaSoporteControversiaActuacion(pControversiaActuacionId, pRutaSoporte, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        

        [HttpPut]
        [Route("CambiarEstadoActuacionSeguimiento")]
        public async Task<IActionResult> CambiarEstadoActuacionSeguimiento(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.CambiarEstadoActuacionSeguimiento(pActuacionSeguimientoId, pEstadoReclamacionCodigo, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPut]
        [Route("CambiarEstadoControversiaActuacion")]
        public async Task<IActionResult> CambiarEstadoControversiaActuacion([FromQuery] int pControversiaActuacionId, string pNuevoCodigoEstadoAvance)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _contractualControversy.CambiarEstadoControversiaActuacion(pControversiaActuacionId, pNuevoCodigoEstadoAvance, HttpContext.User.FindFirst("User").Value);
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
