using System;
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


    }
}
