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

        [HttpPost]
        [Route("CreateEditarModification")]
        public async Task<IActionResult> CreateEditarModification(NovedadContractual novedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (novedadContractual.NovedadContractualId == 0)
                   // novedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                else
                   //novedadContractual.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _contractualModification.CreateEditarModification(novedadContractual);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


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
