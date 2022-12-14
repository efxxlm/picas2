using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JudicialDefenseController : ControllerBase
    {

        public readonly IJudicialDefense _judicialDefense;

        public JudicialDefenseController(IJudicialDefense judicialDefense)
        {
            _judicialDefense = judicialDefense;
        }


        [HttpPost]
        [Route("CreateOrEditDemandadoConvocado")]
        public async Task<IActionResult> CreateOrEditDemandadoConvocado([FromBody] DemandadoConvocado demandadoConvocado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                if (demandadoConvocado.DemandadoConvocadoId == 0)
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
        [Route("GetActuacionesByDefensaJudicialID")]
        public async Task<List<DefensaJudicialSeguimiento>> GetActuacionesByDefensaJudicialID(int pDefensaJudicialId)
        {
            try
            {
                return await _judicialDefense.GetActuacionesByDefensaJudicialID(pDefensaJudicialId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetPlantillaDefensaJudicial")]
        public async Task<FileResult> GetPlantillaDefensaJudicial(int pContratoId, int tipoArchivo)
        {
            return File(await _judicialDefense.GetPlantillaDefensaJudicial(pContratoId, tipoArchivo), "application/pdf");
        }

        [HttpGet]
        [Route("GetVistaDatosBasicosProceso")]
        public async Task<ActionResult<DefensaJudicial>> GetVistaDatosBasicosProceso(int pDefensaJudicialId)
        {
            try
            {
                return await _judicialDefense.GetVistaDatosBasicosProceso(pDefensaJudicialId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateOrEditDefensaJudicial")]
        public async Task<IActionResult> CreateOrEditDefensaJudicial([FromBody] DefensaJudicial defensaJudicial)
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

        /*jflorez deprecated, no entendi su funcionalidad*/
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

        [HttpPut]
        [Route("CambiarEstadoDefensaJudicial")]

        public async Task<IActionResult> CambiarEstadoDefensaJudicial([FromQuery] int pDefensaJudicialId, string pCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _judicialDefense.CambiarEstadoDefensaJudicial(pDefensaJudicialId, pCodigoEstado, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("EliminarDefensaJudicial")]
        [HttpPost]
        public async Task<IActionResult> EliminarDefensaJudicial(int pDefensaJudicialId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _judicialDefense.EliminarDefensaJudicial(pDefensaJudicialId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
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

        /*autor: jflorez
           descripción: trae listado de contratos asignados 
           impacto: CU 4.2.2*/
        [HttpGet]
        [Route("GetListContract")]
        public async Task<ActionResult<List<Contrato>>> GetListContract()
        {
            try
            {
                return await _judicialDefense.GetListContract();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListProyectsByContract")]
        public async Task<ActionResult<List<ProyectoGrilla>>> GetListProyectsByContract(int pContratoId)
        {
            try
            {
                return await _judicialDefense.GetListProyectsByContract(pContratoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Route("EnviarAComite")]
        [HttpPost]
        public async Task<IActionResult> EnviarAComite(int pDefensaJudicialId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _judicialDefense.EnviarAComite(pDefensaJudicialId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("CerrarProceso")]
        [HttpPost]
        public async Task<IActionResult> CerrarProceso(int pDefensaJudicialId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _judicialDefense.CerrarProceso(pDefensaJudicialId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("DeleteActuation")]
        [HttpPost]
        public async Task<IActionResult> DeleteActuation(int id)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _judicialDefense.DeleteActuation(id, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// control el botón “Finalizar actuación”, el cual se habilitará una vez hayan sido diligenciados los campos obligatorios del formulario de actuaciones. 
        /// Al dar clic, el registro no se podrá visualizar más en la grilla de control de las actuaciones. ............?¡?¡?¡?¡?¡¿
        /// </summary>
        /// <param name="pDefensaJudicialId"></param>
        /// <returns></returns>
        [Route("FinalizeActuation")]
        [HttpPost]
        public async Task<IActionResult> FinalizeActuation(int id)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _judicialDefense.FinalizeActuation(id, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CreateOrEditDefensaJudicialSeguimiento")]
        public async Task<IActionResult> CreateOrEditDefensaJudicialSeguimiento(DefensaJudicialSeguimiento defensaJudicialSeguimiento)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                defensaJudicialSeguimiento.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                respuesta = await _judicialDefense.CreateOrEditDefensaJudicialSeguimiento(defensaJudicialSeguimiento);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetDefensaJudicialSeguimiento")]
        public async Task<DefensaJudicialSeguimiento> GetDefensaJudicialSeguimiento(int defensaJudicialSeguimientoId)
        {
            try
            {
                return await _judicialDefense.GetDefensaJudicialSeguimiento(defensaJudicialSeguimientoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("DeleteDemandadoConvocado")]
        public async Task<IActionResult> DeleteDemandadoConvocado([FromQuery] int demandadoConvocadoId, int numeroDemandados)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _judicialDefense.DeleteDemandadoConvocado(demandadoConvocadoId, HttpContext.User.FindFirst("User").Value, numeroDemandados);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteDemandanteConvocante")]
        public async Task<IActionResult> DeleteDemandanteConvocante([FromQuery] int demandanteConvocadoId, int numeroDemandantes)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _judicialDefense.DeleteDemandanteConvocante(demandanteConvocadoId, HttpContext.User.FindFirst("User").Value, numeroDemandantes);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteDefensaJudicialContratacionProyecto")]
        public async Task<IActionResult> DeleteDefensaJudicialContratacionProyecto([FromQuery] int contratacionId, [FromQuery] int defensaJudicialId, int cantContratos)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _judicialDefense.DeleteDefensaJudicialContratacionProyecto(contratacionId, defensaJudicialId, HttpContext.User.FindFirst("User").Value, cantContratos);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato")]
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato([FromQuery] string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            return await _judicialDefense.GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(pTipoSolicitud, pModalidadContrato, pNumeroContrato);
        }



    }
}
