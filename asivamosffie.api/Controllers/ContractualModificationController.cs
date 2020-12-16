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
    public class ContractualModificationController : ControllerBase
    {
        public readonly IContractualModification _contractualModification;

        public ContractualModificationController(IContractualModification contractualModification)
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
        [Route("CreateEditarModification")]
        public async Task<IActionResult> CreateEditarModification(NovedadContractual novedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (novedadContractual.NovedadContractualId == 0)
                {
                    novedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                }
                else
                {
                    respuesta = await _contractualModification.CreateEditarModification(novedadContractual);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        /*autor: jflorez
           descripción: grilla de novedades
           impacto: CU 4.1.3*/
        [HttpGet]
        [Route("GetListGrillaNovedadContractual")]
        public async Task<ActionResult<List<NovedadContractual>>> GetListGrillaNovedadContractual()
        {
            try
            {
                return await _contractualModification.GetListGrillaNovedadContractual();
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
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

    }
}
